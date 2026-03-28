using Ecclesia.Domain.Common.Enums;

namespace Ecclesia.Application.ThirdParty.DTOs;

public record ThirdPartyListDto(
    Guid               Id,
    string?            IdentificationNumber,
    IdentificationType TypeIden,
    PersonType         PersonType,
    string?            FirstName,
    string?            LastName,
    string?            BusinessName,
    string?            Email,
    string?            Phone,
    string?            City,
    string?            Country,
    bool               IsActive,
    DateTime           RegisteredAt,

    // Roles como flags — sin detalle
    bool               IsSupplier,
    bool               IsMember,
    bool               IsDonor,
    bool               IsEmployee,
    bool               IsPartner,
    bool               IsCustomer
);