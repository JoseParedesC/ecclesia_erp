namespace Ecclesia.Application.Users.DTOs;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    DateTime UpdatedAt
);