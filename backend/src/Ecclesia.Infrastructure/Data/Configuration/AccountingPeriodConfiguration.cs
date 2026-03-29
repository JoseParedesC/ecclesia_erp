using Ecclesia.Domain.Common.Constants.SchemaConstants;
using Ecclesia.Domain.Entities.AccountingPeriod;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecclesia.Infrastructure.Data.Configurations;

public class AccountingPeriodEntityConfiguration : IEntityTypeConfiguration<AccountingPeriodEntity>
{
    public void Configure(EntityTypeBuilder<AccountingPeriodEntity> builder)
    {
        builder.ToTable("accounting_periods", schema: SchemaConstants.Accounting.schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Year)
            .IsRequired();

        builder.Property(x => x.Month)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // No puede existir dos períodos con el mismo año y mes
        builder.HasIndex(x => new { x.Year, x.Month })
            .IsUnique();
    }
}