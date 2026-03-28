using Microsoft.EntityFrameworkCore;
using Ecclesia.Domain.Entities.Users;
using Ecclesia.Domain.Entities.Roles;

using Ecclesia.Domain.Entities.JournalVoucher;
using Ecclesia.Domain.Entities.Account;
using Ecclesia.Domain.Entities.Income;
using Ecclesia.Domain.Entities.Expense;
using Ecclesia.Domain.Entities.JournalVoucherLine;
using Ecclesia.Domain.Entities.AccountingPeriod;
using Ecclesia.Domain.Entities.SequenceControl;
using Ecclesia.Domain.Entities.Comunity;
using Ecclesia.Domain.Entities.Accounting;
using Ecclesia.Domain.Entities.ThirdParty;
using Ecclesia.Domain.Entities.ThirdPartyBranches;

namespace Ecclesia.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RoleEntity> Roles => Set<RoleEntity>();
    public DbSet<UserRoleEntity> UserRoles => Set<UserRoleEntity>();
    public DbSet<RolePermissionEntity> RolePermissions => Set<RolePermissionEntity>();


    public DbSet<JournalVoucherEntity> JournalVouchers => Set<JournalVoucherEntity>();
    public DbSet<JournalVoucherLineEntity> JournalVoucherLines=> Set<JournalVoucherLineEntity>();
    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();
    public DbSet<AccountingPeriodEntity> AccountingPeriods => Set<AccountingPeriodEntity>();
    public DbSet<IncomeEntity> Incomes => Set<IncomeEntity>();
    public DbSet<ExpenseEntity> Expenses => Set<ExpenseEntity>();
    public DbSet<SequenceControlEntity> SequenceControls => Set<SequenceControlEntity>();
    public DbSet<CommunityEntity> Communities => Set<CommunityEntity>();
    public DbSet<CashAccountEntity> CashAccounts => Set<CashAccountEntity>();

    public DbSet<ThirdPartyEntity> ThirdParties => Set<ThirdPartyEntity>();
    public DbSet<SupplierInfo> SupplierInfos => Set<SupplierInfo>();
    public DbSet<MemberInfo> MemberInfos => Set<MemberInfo>();
    public DbSet<DonorInfo> DonorInfos => Set<DonorInfo>();
    public DbSet<EmployeeInfo> EmployeeInfos => Set<EmployeeInfo>();
    public DbSet<PartnerInfo> PartnerInfos => Set<PartnerInfo>();
    public DbSet<CustomerInfo> CustomerInfos => Set<CustomerInfo>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automáticamente todas las configuraciones del assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName != null)
                entity.SetTableName(ToSnakeCase(tableName));
        }


        // Aplica a todas las entidades que hereden de BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {

            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.Id))
                    .HasDefaultValueSql("gen_random_uuid()") // función nativa de PostgreSQL
                    .ValueGeneratedOnAdd(); // EF lo marca como autogenerado

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedAt))
                    .HasDefaultValueSql("now()")
                    .ValueGeneratedOnAdd();

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.UpdatedAt))
                    .HasDefaultValueSql("now()")
                    .ValueGeneratedOnAddOrUpdate();

                // modelBuilder.Entity(entityType.ClrType)
                //     .Property(nameof(BaseEntity.RowVersion))
                //     .IsRowVersion();

    
                modelBuilder.Entity(entityType.ClrType) //concurrency xmin UseXminAsConcurrencyToken()
                    .Property<uint>("xmin")
                    .HasColumnName("xmin")
                    .HasColumnType("xid")
                    .ValueGeneratedOnAddOrUpdate()
                    .IsConcurrencyToken();


            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.UpdatedAt = utcNow;
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.CreatedAt).IsModified = false; // nunca se modifica
                    entry.Entity.UpdatedAt = utcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private static string ToSnakeCase(string name)
    {
        return string.Concat(name.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? "_" + c : c.ToString()
        )).ToLower();
    }


}