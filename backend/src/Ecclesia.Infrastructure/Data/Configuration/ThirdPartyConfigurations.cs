using Ecclesia.Domain.Entities.ThirdParty;
using Ecclesia.Domain.Entities.ThirdPartyBranches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ThirdPartyEntityConfiguration : IEntityTypeConfiguration<ThirdPartyEntity>
{
    public void Configure(EntityTypeBuilder<ThirdPartyEntity> builder)
    {
        builder.ToTable("ThirdParties");

        builder.HasKey(x => x.Id);

        // Identificación
        builder.Property(x => x.IdentificationNumber)
            .HasMaxLength(20);

        builder.Property(x => x.TypeIden)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.PersonType)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Persona Natural
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);

        // Persona Jurídica
        builder.Property(x => x.BusinessName).HasMaxLength(200);
        builder.Property(x => x.TradeName).HasMaxLength(200);

        // Contacto
        builder.Property(x => x.Email).HasMaxLength(150);
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.Address).HasMaxLength(300);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.Country).HasMaxLength(100);

        // Ignorar propiedades calculadas
        builder.Ignore(x => x.IsSupplier);
        builder.Ignore(x => x.IsMember);
        builder.Ignore(x => x.IsDonor);
        builder.Ignore(x => x.IsEmployee);
        builder.Ignore(x => x.IsPartner);
        builder.Ignore(x => x.IsCustomer);

        // Relaciones 1 a 1
        builder.HasOne(x => x.Supplier)
            .WithOne()
            .HasForeignKey<SupplierInfo>(x => x.ThirdPartyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Member)
            .WithOne()
            .HasForeignKey<MemberInfo>(x => x.ThirdPartyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Donor)
            .WithOne()
            .HasForeignKey<DonorInfo>(x => x.ThirdPartyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Employee)
            .WithOne()
            .HasForeignKey<EmployeeInfo>(x => x.ThirdPartyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Partner)
            .WithOne()
            .HasForeignKey<PartnerInfo>(x => x.ThirdPartyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Customer)
            .WithOne()
            .HasForeignKey<CustomerInfo>(x => x.ThirdPartyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}