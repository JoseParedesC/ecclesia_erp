using Ecclesia.Domain.Common.Enums;
using Ecclesia.Domain.Entities.ThirdParty;

namespace Ecclesia.Application.ThirdParty.DTOs;

public record ThirdPartyDetailDto(
    Guid               Id,
    string?            IdentificationNumber,
    IdentificationType TypeIden,
    PersonType         PersonType,

    // Persona Natural
    string?            FirstName,
    string?            LastName,
    DateOnly?          BirthDate,

    // Persona Jurídica
    string?            BusinessName,
    string?            TradeName,

    // Contacto
    string?            Email,
    string?            Phone,
    string?            Address,
    string?            City,
    string?            Country,

    // Estado
    bool               IsActive,
    DateTime           RegisteredAt,

    // Roles
    bool               IsSupplier,
    bool               IsMember,
    bool               IsDonor,
    bool               IsEmployee,
    bool               IsPartner,
    bool               IsCustomer,

    // Info por rol
    SupplierDetailDto?  Supplier,
    MemberDetailDto?    Member,
    DonorDetailDto?     Donor,
    EmployeeDetailDto?  Employee,
    PartnerDetailDto?   Partner,
    CustomerDetailDto?  Customer
);

public record SupplierDetailDto(
    string? BankAccount,
    string? BankName,
    int     PaymentTermDays,
    string? TaxRegime
);

public record MemberDetailDto(
    string?      MemberCode,
    DateTime     MemberSince,
    string?      Ministry,
    MemberStatus Status
);

public record DonorDetailDto(
    DateTime  FirstDonationDate,
    DateTime? LastDonationDate,
    decimal   TotalDonated,
    bool      IsRecurrent
);

public record EmployeeDetailDto(
    string?   Position,
    string?   Department,
    DateTime  HireDate,
    DateTime? TerminationDate,
    decimal   Salary,
    string?   BankAccount
);

public record PartnerDetailDto(
    string?     Organization,
    DateTime    PartnerSince,
    PartnerType PartnerType,
    string?     AgreementCode
);

public record CustomerDetailDto(
    string?         CustomerCode,
    decimal         CreditLimit,
    int             PaymentTermDays,
    CustomerSegment Segment,
    DateTime        FirstPurchaseDate
);