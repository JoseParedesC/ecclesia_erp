using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecclesia.Domain.Entities.JournalVoucherLine;
using Ecclesia.Domain.Common.Constants.SchemaConstants;

public class JournalVoucherLineConfiguration : IEntityTypeConfiguration<JournalVoucherLineEntity>
{
    public void Configure(EntityTypeBuilder<JournalVoucherLineEntity> builder)
    {
        builder.ToTable("JournalVoucherLine", schema: SchemaConstants.Ecclesia.schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.LineType)
            .HasConversion<string>();
    }
}