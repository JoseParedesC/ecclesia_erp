using Ecclesia.Domain.Entities.Roles;

namespace Ecclesia.Application.Roles.DTOs;

public static class RolesMapper
{
    public static SearchRoleDto ToSearchDto(this RoleEntity entity) => new(
        entity.Id,
        entity.Name
    );
}