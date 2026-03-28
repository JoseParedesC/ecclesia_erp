using Ecclesia.Domain.Common.Constants.SchemaConstants;
using Ecclesia.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CashAccountConfiguration : IEntityTypeConfiguration<CashAccountEntity>
{
    public void Configure(EntityTypeBuilder<CashAccountEntity> builder)
    {
        builder.ToTable("cash_accounts", schema: SchemaConstants.Accounting.schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}
