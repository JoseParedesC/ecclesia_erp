using Ecclesia.Domain.Common.Constants;
namespace Ecclesia.Domain.Entities.AccountingPeriod;

public class AccountingPeriodEntity : BaseEntity
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public StatusDocument Status { get; set; }
    public DateTime? ClosedAt { get; set; }
    public Guid? ClosedBy { get; set; }
    public Guid? ReopenedBy { get; set; }
    public DateTime? ReopenedAt { get; set; }

    private AccountingPeriodEntity() { }

    public AccountingPeriodEntity(int year, int month)
    {
        Year   = year;
        Month  = month;
        Status = StatusDocument.OPEN;
    }

    public void Close(Guid closedBy)
    {
        Status   = StatusDocument.CLOSED;
        ClosedAt = DateTime.UtcNow;
        ClosedBy = closedBy;
    }

    public void Reopen(Guid reopenedBy)
    {
        Status     = StatusDocument.OPEN;
        ClosedAt   = null;
        ClosedBy   = null;
        ReopenedBy = reopenedBy;
        ReopenedAt = DateTime.UtcNow;
    }

    public bool IsOpen   => Status == StatusDocument.OPEN;
    public bool IsClosed => Status == StatusDocument.CLOSED;

    public DateTime GetPeriodDate() => new DateTime(Year, Month, 1);
}