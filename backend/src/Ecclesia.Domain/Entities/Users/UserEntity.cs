using System.ComponentModel.DataAnnotations.Schema;

namespace Ecclesia.Domain.Entities.Users;

[Table("Users", Schema = "access_manager")]
public class UserEntity: BaseEntity{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

}