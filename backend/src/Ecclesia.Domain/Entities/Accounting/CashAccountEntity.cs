using Ecclesia.Domain.Common;

namespace Ecclesia.Domain.Entities.Accounting;

public class CashAccountEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // CASH / BANK
    public bool IsActive { get; set; } = true;
}