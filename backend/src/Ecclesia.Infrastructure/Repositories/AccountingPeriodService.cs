
using Ecclesia.Domain.Entities.AccountingPeriod;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class AccountingPeriodService : IAccountingPeriodRepository
{
    private readonly AppDbContext _context;

    public AccountingPeriodService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> GetOpenPeriodIdAsync(DateTime date, CancellationToken ct)
    {
        var period = await _context.AccountingPeriods
            .FirstOrDefaultAsync(x =>
                x.Year == date.Year &&
                x.Month == date.Month, ct);

        if (period == null)
            throw new Exception("Accounting period not found");

        if (period.Status == StatusDocument.CLOSED)
            throw new Exception("Accounting period is closed");

        return period.Id;
    }

    public async Task<AccountingPeriodEntity?> GetOpenPeriodAsync(Guid communityId, CancellationToken cancellationToken = default)
        => await _context.AccountingPeriods
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.CommunityId == communityId &&
                p.Status == StatusDocument.OPEN,
                cancellationToken);
}