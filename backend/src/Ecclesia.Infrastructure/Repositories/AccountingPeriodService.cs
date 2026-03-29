
using Ecclesia.Domain.Common;
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


    public async Task<AccountingPeriodEntity> CreateOpenPeriodAsync(AccountingPeriodEntity newPeriod, CancellationToken ct)
    {
        var period = await _context.AccountingPeriods
            .FirstOrDefaultAsync(x =>
                x.Year == newPeriod.Year &&
                x.Month == newPeriod.Month, ct);

        await _context.AccountingPeriods.AddAsync(newPeriod, ct);

        return newPeriod;
    }

    public async Task<AccountingPeriodEntity?> GetPeriodAsync(DateTime date, CancellationToken ct)
    {
        var period = await _context.AccountingPeriods
            .FirstOrDefaultAsync(x =>
                x.Year == date.Year &&
                x.Month == date.Month, ct);

        return period;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);   
    }

}