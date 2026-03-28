
namespace Ecclesia.Application.Accounts.DTOs;

public record AccountDetailDto(
    Guid        Id,
    string?     Code,
    string?     Name,
    AccountType Type,
    Guid?       ParentAccountId,
    string?     ParentAccountName,
    IEnumerable<AccountChildDto> ChildAccounts,
    DateTime    CreatedAt,
    DateTime    UpdatedAt
);

