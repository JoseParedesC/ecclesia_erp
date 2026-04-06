using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.Comunity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecclesia.Domain.Entities;

[Table("rostros", Schema = "org")]
public class RostroEntity : BaseEntity
{
    public string Code    { get; private set; } = default!;
    public string Name    { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool   IsActive { get; private set; } = true;

    // Nav properties
    public ICollection<CommunityEntity> Communities { get; private set; } = [];

    private RostroEntity() { }

    public static RostroEntity Create(string code, string name, string? description = null)
    {
        return new RostroEntity
        {
            Code        = code.Trim().ToUpperInvariant(),
            Name        = name.Trim(),
            Description = description?.Trim(),
            IsActive    = true
        };
    }

    public void Update(string name, string? description)
    {
        Name        = name.Trim();
        Description = description?.Trim();
    }

    public void Deactivate() => IsActive = false;
    public void Activate()   => IsActive = true;
}
