namespace Ecclesia.Application.Auth.DTOs;

public record LoginDto(
    string Email,
    string Password
);