using Ecclesia.Domain.Entities.Account;

namespace Ecclesia.Application.Accounts.DTOs;

public record AccountSearchDto(
    Guid        Id,
    string?     Code,
    string?     Name,
    AccountType Type
);