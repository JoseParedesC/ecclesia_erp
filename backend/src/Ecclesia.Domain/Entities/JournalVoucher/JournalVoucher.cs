using Ecclesia.Domain.Entities.JournalVoucherLine;

namespace Ecclesia.Domain.Entities.JournalVoucher;

public class JournalVoucherEntity : BaseEntity
{
    private readonly List<JournalVoucherLineEntity> _lines = new();

    public string? VoucherNumber { get; private set; }
    public VoucherType Type { get; private set; }
    public VoucherStatus Status { get; private set; }

    public DateTime Date { get; private set; }
    public string? Description { get; private set; }

    public Guid AccountingPeriodId { get; private set; }
    public Guid RostroId { get; private set; }
    public Guid CommunityId { get; private set; }

    public IReadOnlyCollection<JournalVoucherLineEntity> Lines => _lines;

    private JournalVoucherEntity() { }

    public JournalVoucherEntity(
        string voucherNumber,
        VoucherType type,
        DateTime date,
        string description,
        Guid accountingPeriodId,
        Guid rostroId,
        Guid communityId)
    {
        VoucherNumber = voucherNumber;
        Type = type;
        Date = date;
        Description = description;
        AccountingPeriodId = accountingPeriodId;
        RostroId = rostroId;
        CommunityId = communityId;

        Status = VoucherStatus.Draft;
    }

    public void AddLine(Guid accountId, decimal amount, LineType type)
    {
        if (Status != VoucherStatus.Draft)
            throw new InvalidOperationException("Cannot modify a posted voucher");

        _lines.Add(new JournalVoucherLineEntity(Id, accountId, amount, type));
    }

    public void Post()
    {
        if (!_lines.Any())
            throw new InvalidOperationException("Voucher must have lines");

        var debit = _lines.Where(x => x.LineType == LineType.Debit).Sum(x => x.Amount);
        var credit = _lines.Where(x => x.LineType == LineType.Credit).Sum(x => x.Amount);

        if (debit != credit)
            throw new InvalidOperationException("Voucher is not balanced");

        Status = VoucherStatus.Posted;
    }
}