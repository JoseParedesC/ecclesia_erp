
using Microsoft.EntityFrameworkCore;
using Ecclesia.Domain.Entities.JournalVoucher;
using Ecclesia.Domain.Interfaces;
using Ecclesia.Infrastructure.Data;

namespace Ecclesia.Infrastructure.Repositories;

public class JournalVoucherRepository : IJournalVoucherRepository
{
    private readonly AppDbContext _context;

    public JournalVoucherRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(JournalVoucherEntity voucher, CancellationToken cancellationToken = default)
    {
        await _context.JournalVouchers.AddAsync(voucher, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<JournalVoucherEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.JournalVouchers
            .AsNoTracking()
            .Include(j => j.Lines)
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

    public async Task<string> GenerateVoucherNumberAsync(CancellationToken cancellationToken = default)
    {
        var year  = DateTime.UtcNow.Year;
        var month = DateTime.UtcNow.Month;
        var count = await _context.JournalVouchers
            .CountAsync(j => j.Date.Year == year && j.Date.Month == month, cancellationToken);

        return $"JV-{year}{month:D2}-{count + 1:D4}"; // JV-202503-0001
    } 
}