namespace Ecclesia.Application.Users.DTOs;

public record UserSummaryDto(
    Guid Id,
    string Name,
    string Email
);