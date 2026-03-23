
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

    public async Task AddAsync(JournalVoucherEntity voucher, CancellationToken ct)
    {
        await _context.JournalVouchers.AddAsync(voucher, ct);
    }

    public async Task<JournalVoucherEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.JournalVouchers
            .Include("_lines")
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}