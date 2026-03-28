using Ecclesia.Domain.Entities.Account;

namespace Ecclesia.Application.Accounts.DTOs;

public record UpdateAccountDto(
    string?     Code,
    string?     Name,
    AccountType Type,
    Guid?       ParentAccountId
);