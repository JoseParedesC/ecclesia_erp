using System.ComponentModel.DataAnnotations.Schema;
using Ecclesia.Domain.Entities.Roles;

namespace Ecclesia.Domain.Entities.Users;

[Table("Users", Schema = "access_manager")]
public class UserEntity: BaseEntity{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    // Navegación
    public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();

}