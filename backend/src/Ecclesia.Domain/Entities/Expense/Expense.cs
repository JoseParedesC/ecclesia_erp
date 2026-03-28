
namespace   Ecclesia.Domain.Entities.Expense;

public class ExpenseEntity : BaseEntity
{
    public DateTime Date { get; private set; }
    public decimal Amount { get; private set; }

    public Guid CashAccountId { get; private set; }
    public Guid CommunityId { get; private set; }

    public Guid JournalVoucherId { get; private set; }

    private ExpenseEntity() { }

    public ExpenseEntity(DateTime date, decimal amount, Guid cashAccountId, Guid communityId, Guid journalVoucherId)
    {
        Date = date;
        Amount = amount;
        CashAccountId = cashAccountId;
        CommunityId = communityId;
        JournalVoucherId = journalVoucherId;
    }
}