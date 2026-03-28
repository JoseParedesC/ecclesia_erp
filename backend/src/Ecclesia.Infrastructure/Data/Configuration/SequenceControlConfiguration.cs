
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecclesia.Domain.Entities.SequenceControl;

namespace Ecclesia.Infrastructure.Data.Configuration;

public class SequenceControlConfiguration : IEntityTypeConfiguration<SequenceControlEntity>
{
    public void Configure(EntityTypeBuilder<SequenceControlEntity> builder)
    {
        builder.ToTable("sequence_control");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SequenceType)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.SequenceType)
            .IsUnique();
    }
}