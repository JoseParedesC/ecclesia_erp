
using Ecclesia.Domain.Entities.JournalVoucher;

namespace Ecclesia.Domain.Interfaces;

public interface IJournalVoucherRepository
{
    Task AddAsync(JournalVoucherEntity voucher, CancellationToken ct);
    Task<JournalVoucherEntity?> GetByIdAsync(Guid id, CancellationToken ct);
}