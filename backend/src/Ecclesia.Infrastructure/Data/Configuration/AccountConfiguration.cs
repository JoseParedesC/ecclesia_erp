using Ecclesia.Domain.Common.Constants.SchemaConstants;
using Ecclesia.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecclesia.Infrastructure.Data.Configurations;

public class AccountEntityConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.ToTable("accounts", schema: SchemaConstants.Accounting.schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        // Unique constraint — no pueden existir dos cuentas con el mismo código
        builder.HasIndex(x => x.Code)
            .IsUnique();

        // Relación auto-referencial
        builder.HasOne(x => x.ParentAccount)
            .WithMany(x => x.ChildAccounts)
            .HasForeignKey(x => x.ParentAccountId)
            .OnDelete(DeleteBehavior.Restrict) // ← importante: no eliminar en cascada
            .IsRequired(false);
    }
}