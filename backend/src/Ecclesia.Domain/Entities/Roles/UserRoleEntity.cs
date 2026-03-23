using Ecclesia.Domain.Entities.Users;

namespace Ecclesia.Domain.Entities.Roles;

public class UserRoleEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    // Navegación
    public UserEntity User { get; set; } = null!;
    public RoleEntity Role { get; set; } = null!;
}