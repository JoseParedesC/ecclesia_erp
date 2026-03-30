namespace Ecclesia.Application.Roles.Queries.SearchRoles;

public record SearchRoleQuery(
    string? Search,
    int     Page,
    int     PageSize
);