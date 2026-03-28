
namespace Ecclesia.Application.Accounts.DTOs;

public record AccountChildDto(
    Guid        Id,
    string?     Code,
    string?     Name,
    AccountType Type
);