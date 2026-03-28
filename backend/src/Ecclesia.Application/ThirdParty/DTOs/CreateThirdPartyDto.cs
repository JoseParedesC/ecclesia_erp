using Ecclesia.Domain.Common.Enums;
using Ecclesia.Domain.Entities.ThirdParty;

namespace Ecclesia.Application.ThirdParty.DTOs;

public record CreateThirdPartyDto(
    string?            IdentificationNumber,
    IdentificationType TypeIden,
    PersonType         PersonType,
    string?            FirstName,
    string?            LastName,
    DateOnly?          BirthDate,
    string?            BusinessName,
    string?            TradeName,
    string?            Email,
    string?            Phone,
    string?            Address,
    string?            City,
    string?            Country
);