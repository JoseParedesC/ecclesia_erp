using Ecclesia.Domain.Common.Constants.SchemaConstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecclesia.Domain.Entities.Comunity;

public class CommunityConfiguration : IEntityTypeConfiguration<CommunityEntity>
{
    public void Configure(EntityTypeBuilder<CommunityEntity> builder)
    {
        builder.ToTable("community", schema: SchemaConstants.Ecclesia.schema);

        // PK
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.RostroId)
            .IsRequired();

        // Indexes
        builder.HasIndex(x => x.Name);

        // Relaciones
        // builder.HasOne<Rostro>()
        //     .WithMany()
        //     .HasForeignKey(x => x.RostroId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // Opcional: evitar duplicados por Rostro + Name
        builder.HasIndex(x => new { x.RostroId, x.Name })
            .IsUnique();
    }
}