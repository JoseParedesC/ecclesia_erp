
using Ecclesia.Domain.Entities.Income;
using Ecclesia.Domain.Interfaces;
using Ecclesia.Infrastructure.Data;

namespace Ecclesia.Infrastructure.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly AppDbContext _context;

    public IncomeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(IncomeEntity income, CancellationToken ct)
    {
        await _context.Incomes.AddAsync(income, ct);
    }
}