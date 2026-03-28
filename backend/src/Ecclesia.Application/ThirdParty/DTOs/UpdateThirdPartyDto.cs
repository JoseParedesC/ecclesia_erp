namespace Ecclesia.Application.ThirdParty.DTOs;

public record UpdateThirdPartyDto(
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