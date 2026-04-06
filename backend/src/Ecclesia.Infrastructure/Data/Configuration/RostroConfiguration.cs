using Ecclesia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecclesia.Infrastructure.Data.Configurations;

public sealed class RostroConfiguration : IEntityTypeConfiguration<RostroEntity>
{
    public void Configure(EntityTypeBuilder<RostroEntity> builder)
    {
        builder.ToTable("rostros", "org");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasMaxLength(500);

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Índices únicos — segunda línea de defensa tras los handlers
        builder.HasIndex(r => r.Code)
            .IsUnique()
            .HasDatabaseName("ix_rostros_code");

        builder.HasIndex(r => r.Name)
            .IsUnique()
            .HasDatabaseName("ix_rostros_name");

        // Relación con Community — Restrict evita eliminar un Rostro con Communities asociadas
        builder.HasMany(r => r.Communities)
            .WithOne(c => c.Rostro)
            .HasForeignKey(c => c.RostroId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
