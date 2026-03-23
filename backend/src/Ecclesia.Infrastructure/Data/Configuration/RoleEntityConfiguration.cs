using Ecclesia.Domain.Common.Constants;
using Ecclesia.Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecclesia.Domain.Common.Constants.SchemaConstants;

namespace Ecclesia.Infrastructure.Data.Configurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("roles", schema: SchemaConstants.AccessManager.schema);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasMaxLength(256);

        builder.HasIndex(r => r.Name)
            .IsUnique();
    }
}