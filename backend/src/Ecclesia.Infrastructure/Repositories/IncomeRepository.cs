
using Ecclesia.Domain.Entities.Income;
using Ecclesia.Domain.Interfaces;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Common;

namespace Ecclesia.Infrastructure.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly AppDbContext _context;

    public IncomeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(IncomeEntity income, CancellationToken cancellationToken = default)
    {
        await _context.Incomes.AddAsync(income, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IncomeEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Incomes
            .AsNoTracking()
            .Include(i => i.Donor)
            .Include(i => i.CashAccount)
            .Include(i => i.JournalVoucher)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task<PagedResult<IncomeEntity>> ListAllAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var dbQuery = _context.Incomes
            .AsNoTracking()
            .Include(i => i.Donor)
            .Include(i => i.CashAccount)
            .AsQueryable();

        // Filtro de búsqueda
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search}%";

            dbQuery = !string.IsNullOrWhiteSpace(query.SearchField)
                ? query.SearchField.ToLower() switch  // busca solo en el campo especificado
                {
                    "name"     => dbQuery.Where(u => EF.Functions.ILike(u.Donor.Name,     pattern)),
                    _          => dbQuery
                }
                : dbQuery.Where(u =>  // busca en todos los campos
                    EF.Functions.ILike(u.Donor.Name,     pattern)
                );
        }

        dbQuery = query.OrderBy?.ToLower() switch
        {
            "date"   => query.OrderDescending ? dbQuery.OrderByDescending(i => i.Date)   : dbQuery.OrderBy(i => i.Date),
            "amount" => query.OrderDescending ? dbQuery.OrderByDescending(i => i.Amount) : dbQuery.OrderBy(i => i.Amount),
            _        => dbQuery.OrderByDescending(i => i.Date)
        };

        var totalCount = await dbQuery.CountAsync(cancellationToken);

        var items = await dbQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<IncomeEntity>(items, totalCount, query.Page, query.PageSize);
    }

}