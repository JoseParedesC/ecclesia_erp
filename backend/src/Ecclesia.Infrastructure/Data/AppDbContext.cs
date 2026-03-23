using Microsoft.EntityFrameworkCore;
using Ecclesia.Domain.Entities.Users;

namespace Ecclesia.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Define your DbSets here, for example:
    // public DbSet<YourEntity> YourEntities { get; set; }

    public DbSet<UserEntity> Users { get; set; }


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