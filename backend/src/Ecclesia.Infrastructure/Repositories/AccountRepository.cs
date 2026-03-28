using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities.Account;

namespace Ecclesia.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    // ── READ ──────────────────────────────────────────────────────────────────

    public async Task<AccountEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Accounts
            .AsNoTracking()
            .Include(x => x.ParentAccount)
            .Include(x => x.ChildAccounts)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<AccountEntity>> ListAllAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<AccountEntity> dbQuery = _context.Accounts.AsNoTracking();

        // Filtro de búsqueda
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search}%";

            dbQuery = !string.IsNullOrWhiteSpace(query.SearchField)
                ? query.SearchField.ToLower() switch
                {
                    "code" => dbQuery.Where(x => EF.Functions.ILike(x.Code ?? string.Empty, pattern)),
                    "name" => dbQuery.Where(x => EF.Functions.ILike(x.Name ?? string.Empty, pattern)),
                    _      => dbQuery
                }
                : dbQuery.Where(x =>
                    EF.Functions.ILike(x.Code ?? string.Empty, pattern) ||
                    EF.Functions.ILike(x.Name ?? string.Empty, pattern)
                );
        }

        // Ordenamiento
        dbQuery = query.OrderBy?.ToLower() switch
        {
            "code" => query.OrderDescending ? dbQuery.OrderByDescending(x => x.Code) : dbQuery.OrderBy(x => x.Code),
            "name" => query.OrderDescending ? dbQuery.OrderByDescending(x => x.Name) : dbQuery.OrderBy(x => x.Name),
            "type" => query.OrderDescending ? dbQuery.OrderByDescending(x => x.Type) : dbQuery.OrderBy(x => x.Type),
            _      => dbQuery.OrderBy(x => x.Code)
        };

        var totalCount = await dbQuery.CountAsync(cancellationToken);

        var items = await dbQuery
            .Include(x => x.ParentAccount)
            .Include(x => x.ChildAccounts)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<AccountEntity>(items, totalCount, query.Page, query.PageSize);
    }

    // ── CREATE ────────────────────────────────────────────────────────────────

    public async Task<AccountEntity> CreateAsync(AccountEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Accounts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    // ── UPDATE ────────────────────────────────────────────────────────────────

    public async Task<AccountEntity> UpdateAsync(AccountEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Accounts.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    // ── DELETE ────────────────────────────────────────────────────────────────

    public async Task<AccountEntity> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Accounts
            .Include(x => x.ParentAccount)
            .Include(x => x.ChildAccounts)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"Account con Id {id} no encontrado.");

        _context.Accounts.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }
}