using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Common.Constants;
using Ecclesia.Domain.Entities.AccountingPeriod;
using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecclesia.Infrastructure.Repositories;

public class AccountingPeriodRepository : IAccountingPeriodRepository
{
    private readonly AppDbContext _context;

    public AccountingPeriodRepository(AppDbContext context)
    {
        _context = context;
    }

    // ── READ ──────────────────────────────────────────────────────────────────

    public async Task<AccountingPeriodEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.AccountingPeriods
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<AccountingPeriodEntity?> GetByYearMonthAsync(int year, int month, CancellationToken cancellationToken = default)
        => await _context.AccountingPeriods
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Year == year && x.Month == month, cancellationToken);

    public async Task<AccountingPeriodEntity?> GetCurrentOpenAsync(CancellationToken cancellationToken = default)
        => await _context.AccountingPeriods
            .AsNoTracking()
            .Where(x => x.Status == StatusDocument.OPEN)
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<PagedResult<AccountingPeriodEntity>> ListAllAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<AccountingPeriodEntity> dbQuery = _context.AccountingPeriods.AsNoTracking();

        // Filtro de búsqueda por status
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search}%";
            dbQuery = dbQuery.Where(x =>
                EF.Functions.ILike(x.Status.ToString(), pattern)
            );
        }

        // Ordenamiento
        dbQuery = query.OrderBy?.ToLower() switch
        {
            "year"  => query.OrderDescending ? dbQuery.OrderByDescending(x => x.Year)  : dbQuery.OrderBy(x => x.Year),
            "month" => query.OrderDescending ? dbQuery.OrderByDescending(x => x.Month) : dbQuery.OrderBy(x => x.Month),
            _       => dbQuery.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
        };

        var totalCount = await dbQuery.CountAsync(cancellationToken);

        var items = await dbQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<AccountingPeriodEntity>(items, totalCount, query.Page, query.PageSize);
    }

    public async Task<bool> ExistsByYearMonthAsync(int year, int month, CancellationToken cancellationToken = default)
        => await _context.AccountingPeriods
            .AsNoTracking()
            .AnyAsync(x => x.Year == year && x.Month == month, cancellationToken);

    // ── CREATE ────────────────────────────────────────────────────────────────

    public async Task<AccountingPeriodEntity> CreateAsync(AccountingPeriodEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.AccountingPeriods.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    // ── UPDATE ────────────────────────────────────────────────────────────────

    public async Task<AccountingPeriodEntity> UpdateAsync(AccountingPeriodEntity entity, CancellationToken cancellationToken = default)
    {
        _context.AccountingPeriods.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }
}