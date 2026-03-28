using Ecclesia.Domain.Common.Constants.SchemaConstants;
using Ecclesia.Domain.Entities.Income;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Ecclesia.Infrastructure.Data.Configurations;

public class IncomeEntityConfiguration : IEntityTypeConfiguration<IncomeEntity>
{
    public void Configure(EntityTypeBuilder<IncomeEntity> builder)
    {
        builder.ToTable("incomes", schema: SchemaConstants.Accounting.schema);

        builder.Property(i => i.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.Date)
            .IsRequired();

        builder.HasOne(i => i.ThirdParty)
            .WithMany()
            .HasForeignKey(i => i.ThirdPartyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.CashAccount)
            .WithMany()
            .HasForeignKey(i => i.CashAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.JournalVoucher)
            .WithMany()
            .HasForeignKey(i => i.JournalVoucherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}