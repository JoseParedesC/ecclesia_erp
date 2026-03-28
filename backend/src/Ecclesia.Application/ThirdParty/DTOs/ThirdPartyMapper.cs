using Ecclesia.Domain.Entities.ThirdParty;
using Ecclesia.Domain.Entities.ThirdPartyBranches;

namespace Ecclesia.Application.ThirdParty.DTOs;

public static class ThirdPartyMapper
{
    public static ThirdPartyDetailDto ToDto(this ThirdPartyEntity entity) => new(
        entity.Id,
        entity.IdentificationNumber,
        entity.TypeIden,
        entity.PersonType,
        entity.FirstName,
        entity.LastName,
        entity.BirthDate,
        entity.BusinessName,
        entity.TradeName,
        entity.Email,
        entity.Phone,
        entity.Address,
        entity.City,
        entity.Country,
        entity.IsActive,
        entity.RegisteredAt,
        entity.IsSupplier,
        entity.IsMember,
        entity.IsDonor,
        entity.IsEmployee,
        entity.IsPartner,
        entity.IsCustomer,
        entity.Supplier?.ToDto(),
        entity.Member?.ToDto(),
        entity.Donor?.ToDto(),
        entity.Employee?.ToDto(),
        entity.Partner?.ToDto(),
        entity.Customer?.ToDto()
    );

    public static SupplierDetailDto ToDto(this SupplierInfo s) => new(
        s.BankAccount,
        s.BankName,
        s.PaymentTermDays,
        s.TaxRegime
    );

    public static MemberDetailDto ToDto(this MemberInfo m) => new(
        m.MemberCode,
        m.MemberSince,
        m.Ministry,
        m.Status
    );

    public static DonorDetailDto ToDto(this DonorInfo d) => new(
        d.FirstDonationDate,
        d.LastDonationDate,
        d.TotalDonated,
        d.IsRecurrent
    );

    public static EmployeeDetailDto ToDto(this EmployeeInfo e) => new(
        e.Position,
        e.Department,
        e.HireDate,
        e.TerminationDate,
        e.Salary,
        e.BankAccount
    );

    public static PartnerDetailDto ToDto(this PartnerInfo p) => new(
        p.Organization,
        p.PartnerSince,
        p.PartnerType,
        p.AgreementCode
    );

    public static CustomerDetailDto ToDto(this CustomerInfo c) => new(
        c.CustomerCode,
        c.CreditLimit,
        c.PaymentTermDays,
        c.Segment,
        c.FirstPurchaseDate
    );

    public static ThirdPartyListDto ToListDto(this ThirdPartyEntity entity) => new(
        entity.Id,
        entity.IdentificationNumber,
        entity.TypeIden,
        entity.PersonType,
        entity.FirstName,
        entity.LastName,
        entity.BusinessName,
        entity.Email,
        entity.Phone,
        entity.City,
        entity.Country,
        entity.IsActive,
        entity.RegisteredAt,
        entity.IsSupplier,
        entity.IsMember,
        entity.IsDonor,
        entity.IsEmployee,
        entity.IsPartner,
        entity.IsCustomer
    );
}