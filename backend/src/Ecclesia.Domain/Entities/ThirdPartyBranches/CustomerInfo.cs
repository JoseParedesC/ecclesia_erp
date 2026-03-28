
using Ecclesia.Domain.Common.Enums;

namespace Ecclesia.Domain.Entities.ThirdPartyBranches;

// Cliente
public class CustomerInfo : BaseEntity
{
    public Guid ThirdPartyId { get; private set; }
    public string? CustomerCode { get; private set; }
    public decimal CreditLimit { get; private set; }
    public int PaymentTermDays { get; private set; }
    public CustomerSegment Segment { get; private set; }
    public DateTime FirstPurchaseDate { get; private set; }

    protected CustomerInfo() { }

    public CustomerInfo(Guid thirdPartyId, string? customerCode, decimal creditLimit, int paymentTermDays, CustomerSegment segment)
    {
        ThirdPartyId = thirdPartyId;
        CustomerCode = customerCode;
        CreditLimit = creditLimit;
        PaymentTermDays = paymentTermDays;
        Segment = segment;
        FirstPurchaseDate = DateTime.UtcNow;
    }
}