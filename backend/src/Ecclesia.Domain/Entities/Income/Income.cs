
namespace Ecclesia.Domain.Entities.Income;

public class IncomeEntity : BaseEntity
{
    public DateTime Date { get; private set; }
    public decimal Amount { get; private set; }

    public Guid? DonorId { get; private set; }
    public Guid CashAccountId { get; private set; }
    public Guid CommunityId { get; private set; }

    public Guid JournalVoucherId { get; private set; }

    private IncomeEntity() { }

    public IncomeEntity(DateTime date, decimal amount, Guid cashAccountId, Guid communityId, Guid journalVoucherId, Guid? donorId = null)
    {
        Date = date;
        Amount = amount;
        CashAccountId = cashAccountId;
        CommunityId = communityId;
        JournalVoucherId = journalVoucherId;
        DonorId = donorId;
    }
}