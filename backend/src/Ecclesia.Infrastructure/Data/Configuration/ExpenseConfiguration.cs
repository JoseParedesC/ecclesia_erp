using Ecclesia.Domain.Common.Constants.SchemaConstants;
using Ecclesia.Domain.Entities.Expense;
using Ecclesia.Domain.Entities.JournalVoucher;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExpenseConfiguration : IEntityTypeConfiguration<ExpenseEntity>
{
    public void Configure(EntityTypeBuilder<ExpenseEntity> builder)
    {
        builder.ToTable("expenses", schema: SchemaConstants.Accounting.schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CashAccountId)
            .IsRequired();

        builder.Property(x => x.CommunityId)
            .IsRequired();

        builder.Property(x => x.JournalVoucherId)
            .IsRequired();

        builder.HasOne<JournalVoucherEntity>()
            .WithMany()
            .HasForeignKey(x => x.JournalVoucherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
