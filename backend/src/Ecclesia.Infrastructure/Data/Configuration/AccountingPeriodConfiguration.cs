using Ecclesia.Domain.Common.Constants.SchemaConstants;
using Ecclesia.Domain.Entities.AccountingPeriod;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AccountingPeriodConfiguration : IEntityTypeConfiguration<AccountingPeriodEntity>
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
            .IsRequired();

        builder.Property(x => x.CommunityId)
            .IsRequired();
    }
}
