using Ecclesia.Domain.Common.Constants.SchemaConstants;
using Ecclesia.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DonorConfiguration : IEntityTypeConfiguration<DonorEntity>
{
    public void Configure(EntityTypeBuilder<DonorEntity> builder)
    {
        builder.ToTable("donors", schema: SchemaConstants.Ecclesia.schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.DocumentNumber)
            .IsRequired();

        builder.Property(x => x.IsCompany)
            .IsRequired();
    }
}
