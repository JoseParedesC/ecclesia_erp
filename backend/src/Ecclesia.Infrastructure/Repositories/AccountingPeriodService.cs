
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class AccountingPeriodService : IAccountingPeriodService
{
    private readonly AppDbContext _context;

    public async Task<Guid> GetOpenPeriodIdAsync(DateTime date, CancellationToken ct)
    {
        var period = await _context.AccountingPeriods
            .FirstOrDefaultAsync(x =>
                x.Year == date.Year &&
                x.Month == date.Month, ct);

        if (period == null)
            throw new Exception("Accounting period not found");

        if (period.IsClosed)
            throw new Exception("Accounting period is closed");

        return period.Id;
    }
}