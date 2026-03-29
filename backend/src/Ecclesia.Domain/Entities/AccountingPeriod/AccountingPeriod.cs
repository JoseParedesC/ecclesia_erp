
using Ecclesia.Domain.Common.Constants;
namespace Ecclesia.Domain.Entities.AccountingPeriod;

public class AccountingPeriodEntity : BaseEntity
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public StatusDocument Status { get; set; } // OPEN / CLOSED
    public DateTime? ClosedAt { get; set; }
    public Guid CommunityId { get; set; }

    private AccountingPeriodEntity() { }

    public AccountingPeriodEntity(int year, int month)
    {
        Year = year;
        Month = month;
        Status = StatusDocument.OPEN;
    }

    public void Close()
    {
        Status = StatusDocument.CLOSED;
    }

    public DateTime GetPeriodDate() => new DateTime(Year, Month, 1);

}