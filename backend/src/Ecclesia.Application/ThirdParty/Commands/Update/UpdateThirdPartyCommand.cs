namespace Ecclesia.Application.ThirdParty.Commands.UpdateThirdParty;

public record UpdateThirdPartyCommand(
    Guid    Id,
    string? FirstName,
    string? LastName,
    string? BusinessName,
    string? TradeName,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? Country
);