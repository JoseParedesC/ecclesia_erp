using Ecclesia.Domain.Common.Constants;
using Ecclesia.Domain.Common.Constants.SchemaConstants;
using Ecclesia.Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecclesia.Infrastructure.Data.Configurations;

public class RolePermissionEntityConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
{
    public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.ToTable("role_permissions", schema: SchemaConstants.AccessManager.schema);

        builder.Property(rp => rp.Schema)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(rp => rp.Option)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(rp => rp.Permission)
            .IsRequired()
            .HasMaxLength(10);

        // FK -> roles
        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice único para evitar permisos duplicados en el mismo rol
        builder.HasIndex(rp => new { rp.RoleId, rp.Schema, rp.Option, rp.Permission })
            .IsUnique();
    }
}