using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecclesia.Domain.Entities.JournalVoucherLine;
using Ecclesia.Domain.Common.Constants.SchemaConstants;

public class JournalVoucherLineConfiguration : IEntityTypeConfiguration<JournalVoucherLineEntity>
{
    public void Configure(EntityTypeBuilder<JournalVoucherLineEntity> builder)
    {
        builder.ToTable("journal_voucher_lines", schema: SchemaConstants.Accounting.schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.LineType)
            .HasConversion<string>();

        builder.HasOne(l => l.Account)
            .WithMany()
            .HasForeignKey(l => l.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}