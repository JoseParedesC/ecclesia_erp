namespace Ecclesia.Application.Auth.DTOs;

public record MeDto(
    Guid UserId,
    string Name,
    string UserName,
    string Email,
    IEnumerable<string> Roles,
    IEnumerable<string> Permissions
);