namespace Ecclesia.Domain.Entities.Roles;

public class RolePermissionEntity : BaseEntity
{
    public string Schema { get; set; } = string.Empty;
    public string Option { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;
    public Guid RoleId { get; set; }

    // Navegación
    public RoleEntity Role { get; set; } = null!;

    public RolePermissionEntity()
    {
    }

    public RolePermissionEntity(Guid roleId, string schema, string option, string permission)
    {
        RoleId = roleId;
        Schema = schema;
        Option = option;
        Permission = permission;
    }
}