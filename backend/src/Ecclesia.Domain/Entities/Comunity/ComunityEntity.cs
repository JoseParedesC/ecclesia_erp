
namespace Ecclesia.Domain.Entities.Comunity;

public class CommunityEntity
{
    public Guid Id { get; private set; }
    public string? Name { get; private set; }
    public Guid RostroId { get; private set; }
    public RostroEntity? Rostro { get; private set; }
}