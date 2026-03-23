namespace Ecclesia.Application.Users.DTOs;

public record CreateUserDto(
    string Name,
    string Email,
    string Password
);