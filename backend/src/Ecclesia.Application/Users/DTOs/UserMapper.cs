using Ecclesia.Domain.Entities.Users;

namespace Ecclesia.Application.Users.DTOs;

public static class UserMapper
{
    public static UserDto ToDto(this UserEntity entity) => new(
        entity.Id,
        entity.Name,
        entity.Email,
        entity.CreatedAt,
        entity.UpdatedAt
    );

    public static UserSummaryDto ToSummaryDto(this UserEntity entity) => new(
        entity.Id,
        entity.Name,
        entity.Email
    );

    public static UserEntity ToEntity(this CreateUserDto dto, string passwordHash) => new()
    {
        Name = dto.Name,
        Email = dto.Email,
        PasswordHash = passwordHash
    };

    
}