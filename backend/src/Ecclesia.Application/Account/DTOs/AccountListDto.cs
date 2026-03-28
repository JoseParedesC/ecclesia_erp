
namespace Ecclesia.Application.Accounts.DTOs;

public record AccountListDto(
    Guid        Id,
    string?     Code,
    string?     Name,
    AccountType Type,
    Guid?       ParentAccountId,
    string?     ParentAccountName,
    DateTime    CreatedAt
);