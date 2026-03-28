namespace Ecclesia.Application.Accounts.DTOs;

public record SearchAccountDto(
    string? Search    = null,
    int     Page      = 1,
    int     PageSize  = 10
);