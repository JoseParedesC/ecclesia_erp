using Ecclesia.Domain.Entities.Roles;

namespace Ecclesia.Application.Roles.DTOs;

public static class RoleMapper
{
    public static RoleDto ToDto(this RoleEntity entity) => new(
        entity.Id,
        entity.Name,
        entity.Description
    );

    public static RoleEntity ToEntity(this CreateRoleDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description
    };
}