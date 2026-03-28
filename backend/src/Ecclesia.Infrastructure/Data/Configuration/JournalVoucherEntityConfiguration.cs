using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecclesia.Domain.Entities.JournalVoucher;
using Ecclesia.Domain.Entities.JournalVoucherLine;
using Ecclesia.Domain.Common.Constants.SchemaConstants;

public class JournalVoucherConfiguration : IEntityTypeConfiguration<JournalVoucherEntity>
{
    public void Configure(EntityTypeBuilder<JournalVoucherEntity> builder)
    {
        builder.ToTable("journal_vouchers", schema: SchemaConstants.Accounting.schema);

        builder.Property(j => j.VoucherNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(j => j.Type)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(j => j.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(j => j.Description)
            .HasMaxLength(256);

        builder.HasIndex(j => j.VoucherNumber)
            .IsUnique();

        builder.HasOne(j => j.AccountingPeriod)
            .WithMany()
            .HasForeignKey(j => j.AccountingPeriodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(j => j.Lines)
            .WithOne(l => l.JournalVoucher)
            .HasForeignKey(l => l.JournalVoucherId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}