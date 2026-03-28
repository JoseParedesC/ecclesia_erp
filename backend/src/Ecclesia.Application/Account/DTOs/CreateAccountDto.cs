using Ecclesia.Domain.Entities.Account;

namespace Ecclesia.Application.Accounts.DTOs;

public record CreateAccountDto(
    string?     Code,
    string?     Name,
    AccountType Type,
    Guid?       ParentAccountId
);