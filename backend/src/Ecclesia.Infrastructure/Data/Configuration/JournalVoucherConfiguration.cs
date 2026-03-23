using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecclesia.Domain.Entities.JournalVoucher;
using Ecclesia.Domain.Entities.JournalVoucherLine;
using Ecclesia.Domain.Common.Constants.SchemaConstants;

public class JournalVoucherConfiguration : IEntityTypeConfiguration<JournalVoucherEntity>
{
    public void Configure(EntityTypeBuilder<JournalVoucherEntity> builder)
    {
        builder.ToTable("JournalVoucher", schema: SchemaConstants.Ecclesia.schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.VoucherNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.VoucherNumber)
            .IsUnique();

        builder.Property(x => x.Type)
            .HasConversion<string>();

        builder.Property(x => x.Status)
            .HasConversion<string>();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder
            .Navigation(e => e.Lines)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
            // .HasForeignKey("JournalVoucherId")
            // .OnDelete(DeleteBehavior.Cascade);
    }
}