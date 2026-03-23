using Ecclesia.Domain.Common;

namespace Ecclesia.Domain.Entities.Accounting;

public class DonorEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public bool IsCompany { get; set; }
}