
namespace Ecclesia.Domain.Entities.SequenceControl;
public class SequenceControlEntity
{
    public Guid Id { get; set; }

    public string SequenceType { get; set; } = default!;
    public int CurrentValue { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}