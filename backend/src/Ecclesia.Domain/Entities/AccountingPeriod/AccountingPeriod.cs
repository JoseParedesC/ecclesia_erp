
namespace Ecclesia.Domain.Entities.AccountingPeriod;

public class AccountingPeriodEntity : BaseEntity
{
    public int Year { get; private set; }
    public int Month { get; private set; }

    public bool IsClosed { get; private set; }

    private AccountingPeriodEntity() { }

    public AccountingPeriodEntity(int year, int month)
    {
        Year = year;
        Month = month;
        IsClosed = false;
    }

    public void Close()
    {
        IsClosed = true;
    }
}