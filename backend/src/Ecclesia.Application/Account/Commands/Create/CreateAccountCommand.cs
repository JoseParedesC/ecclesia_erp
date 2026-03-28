using Ecclesia.Domain.Entities.Account;

namespace Ecclesia.Application.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(
    string?     Code,
    string?     Name,
    AccountType Type,
    Guid?       ParentAccountId
);