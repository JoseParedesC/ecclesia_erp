namespace Ecclesia.Application.Auth.DTOs;

public record AuthResponseDto(
    Guid UserId,
    string Name,
    string UserName,
    string Email,
    string Token,
    DateTime ExpiresAt
);