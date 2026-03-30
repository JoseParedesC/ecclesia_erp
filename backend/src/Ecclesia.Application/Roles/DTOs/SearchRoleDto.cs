namespace Ecclesia.Application.Roles.DTOs;

public record SearchRoleDto(
    Guid? Id          = null,
    string? Search    = null,
    int     Page      = 1,
    int     PageSize  = 10
);
