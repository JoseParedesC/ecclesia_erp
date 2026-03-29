namespace Ecclesia.Domain.Entities.Roles;

public class RoleEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Navegación
    public ICollection<UserRoleEntity>? UserRoles { get; set; }
    public ICollection<RolePermissionEntity>? Permissions { get; set; }

    public RoleEntity() { }

    public RoleEntity(string name, string description = "")
    {
        Name = name;
        Description = description;
    }
}