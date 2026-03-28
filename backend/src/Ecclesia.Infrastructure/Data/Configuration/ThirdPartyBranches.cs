using Ecclesia.Domain.Entities.ThirdPartyBranches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SupplierInfoConfiguration : IEntityTypeConfiguration<SupplierInfo>
{
    public void Configure(EntityTypeBuilder<SupplierInfo> builder)
    {
        builder.ToTable("SupplierInfos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BankAccount).HasMaxLength(50);
        builder.Property(x => x.BankName).HasMaxLength(100);
        builder.Property(x => x.TaxRegime).HasMaxLength(50);
    }
}

public class MemberInfoConfiguration : IEntityTypeConfiguration<MemberInfo>
{
    public void Configure(EntityTypeBuilder<MemberInfo> builder)
    {
        builder.ToTable("MemberInfos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.MemberCode).HasMaxLength(20);
        builder.Property(x => x.Ministry).HasMaxLength(100);
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}

public class DonorInfoConfiguration : IEntityTypeConfiguration<DonorInfo>
{
    public void Configure(EntityTypeBuilder<DonorInfo> builder)
    {
        builder.ToTable("DonorInfos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TotalDonated)
            .HasPrecision(18, 2);
    }
}

public class EmployeeInfoConfiguration : IEntityTypeConfiguration<EmployeeInfo>
{
    public void Configure(EntityTypeBuilder<EmployeeInfo> builder)
    {
        builder.ToTable("EmployeeInfos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Position).HasMaxLength(100);
        builder.Property(x => x.Department).HasMaxLength(100);
        builder.Property(x => x.BankAccount).HasMaxLength(50);
        builder.Property(x => x.Salary)
            .HasPrecision(18, 2);
    }
}

public class PartnerInfoConfiguration : IEntityTypeConfiguration<PartnerInfo>
{
    public void Configure(EntityTypeBuilder<PartnerInfo> builder)
    {
        builder.ToTable("PartnerInfos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Organization).HasMaxLength(200);
        builder.Property(x => x.AgreementCode).HasMaxLength(50);
        builder.Property(x => x.PartnerType)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}

public class CustomerInfoConfiguration : IEntityTypeConfiguration<CustomerInfo>
{
    public void Configure(EntityTypeBuilder<CustomerInfo> builder)
    {
        builder.ToTable("CustomerInfos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CustomerCode).HasMaxLength(20);
        builder.Property(x => x.CreditLimit)
            .HasPrecision(18, 2);
        builder.Property(x => x.Segment)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}