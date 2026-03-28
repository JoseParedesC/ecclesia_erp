using Ecclesia.Domain.Entities.Account;

namespace Ecclesia.Application.Accounts.Commands.UpdateAccount;

public record UpdateAccountCommand(
    Guid        Id,
    string?     Code,
    string?     Name,
    AccountType Type,
    Guid?       ParentAccountId
);