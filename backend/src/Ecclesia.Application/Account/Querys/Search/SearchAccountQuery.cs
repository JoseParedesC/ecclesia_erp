namespace Ecclesia.Application.Accounts.Queries.SearchAccounts;

public record SearchAccountQuery(
    string? Search,
    int     Page,
    int     PageSize
);