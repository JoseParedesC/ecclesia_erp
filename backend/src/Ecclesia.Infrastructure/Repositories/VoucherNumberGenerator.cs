
using Ecclesia.Domain.Entities.SequenceControl;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecclesia.Infrastructure.Repositories;

public class VoucherNumberGenerator : IVoucherNumberGenerator
{
    private readonly AppDbContext _context;

    public VoucherNumberGenerator(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateAsync(VoucherType type, DateTime date, CancellationToken ct)
    {
        var prefix = type.ToString().Substring(0, 3).ToUpper(); // INC, EXP

        var year = date.Year;

        var sequence = await _context.SequenceControls
            .FirstOrDefaultAsync(x => x.SequenceType == type.ToString(), ct);

        if (sequence == null)
        {
            sequence = new SequenceControlEntity
            {
                Id = Guid.NewGuid(),
                SequenceType = type.ToString(),
                CurrentValue = 1
            };

            _context.Add(sequence);
        }
        else
        {
            sequence.CurrentValue++;
        }

        return $"{prefix}-{year}-{sequence.CurrentValue:D6}";
    }
}