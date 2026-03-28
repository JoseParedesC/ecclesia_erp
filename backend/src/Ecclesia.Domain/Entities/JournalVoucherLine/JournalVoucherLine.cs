
using Ecclesia.Domain.Entities.Account;
using Ecclesia.Domain.Entities.JournalVoucher;
namespace Ecclesia.Domain.Entities.JournalVoucherLine;

public class JournalVoucherLineEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid JournalVoucherId { get; private set; }
    public Guid AccountId { get; private set; }

    public decimal Amount { get; private set; }
    public LineType LineType { get; private set; }

    public JournalVoucherEntity JournalVoucher { get; set; } = null!;
    public AccountEntity Account { get; set; } = null!;

    private JournalVoucherLineEntity() { }

    public JournalVoucherLineEntity(Guid voucherId, Guid accountId, decimal amount, LineType type)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        JournalVoucherId = voucherId;
        AccountId = accountId;
        Amount = amount;
        LineType = type;
    }
}