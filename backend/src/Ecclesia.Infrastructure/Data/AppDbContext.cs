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

namespace Ecclesia.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Define your DbSets here, for example:
    // public DbSet<YourEntity> YourEntities { get; set; }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<RolePermissionEntity> RolePermissions { get; set; }


    public DbSet<JournalVoucherEntity> JournalVouchers => Set<JournalVoucherEntity>();
    public DbSet<JournalVoucherLineEntity> JournalVoucherLines => Set<JournalVoucherLineEntity>();
    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();
    public DbSet<AccountingPeriodEntity> AccountingPeriods => Set<AccountingPeriodEntity>();
    public DbSet<IncomeEntity> Incomes => Set<IncomeEntity>();
    public DbSet<ExpenseEntity> Expenses => Set<ExpenseEntity>();
    public DbSet<SequenceControlEntity> SequenceControls { get; set; }
    public DbSet<CommunityEntity> Communities => Set<CommunityEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automáticamente todas las configuraciones del assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

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


}