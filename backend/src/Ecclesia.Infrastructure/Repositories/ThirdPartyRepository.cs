using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities.Accounting;
using Ecclesia.Domain.Entities.ThirdParty;

namespace Ecclesia.Infrastructure.Repositories;

public class ThirdPartyRepository : IThirdPartyRepository
{
    private readonly AppDbContext _context;

    public ThirdPartyRepository(AppDbContext context)
    {
        _context = context;
    }


     // ── READ ──────────────────────────────────────────────────────────────────

    public async Task<ThirdPartyEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.ThirdParties
            .AsNoTracking()
            .Include(x => x.Supplier)
            .Include(x => x.Member)
            .Include(x => x.Donor)
            .Include(x => x.Employee)
            .Include(x => x.Partner)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<ThirdPartyEntity>> ListAllAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        // Base sin Include para permitir filtros y ordenamiento
        IQueryable<ThirdPartyEntity> dbQuery = _context.ThirdParties.AsNoTracking();

        // Filtro de búsqueda
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search}%";

            dbQuery = !string.IsNullOrWhiteSpace(query.SearchField)
                ? query.SearchField.ToLower() switch
                {
                    "firstname"            => dbQuery.Where(x => EF.Functions.ILike(x.FirstName            ?? string.Empty, pattern)),
                    "lastname"             => dbQuery.Where(x => EF.Functions.ILike(x.LastName             ?? string.Empty, pattern)),
                    "businessname"         => dbQuery.Where(x => EF.Functions.ILike(x.BusinessName         ?? string.Empty, pattern)),
                    "identificationnumber" => dbQuery.Where(x => EF.Functions.ILike(x.IdentificationNumber ?? string.Empty, pattern)),
                    "email"                => dbQuery.Where(x => EF.Functions.ILike(x.Email                ?? string.Empty, pattern)),
                    "phone"                => dbQuery.Where(x => EF.Functions.ILike(x.Phone                ?? string.Empty, pattern)),
                    "city"                 => dbQuery.Where(x => EF.Functions.ILike(x.City                 ?? string.Empty, pattern)),
                    _                      => dbQuery
                }
                : dbQuery.Where(x =>
                    EF.Functions.ILike(x.FirstName            ?? string.Empty, pattern) ||
                    EF.Functions.ILike(x.LastName             ?? string.Empty, pattern) ||
                    EF.Functions.ILike(x.BusinessName         ?? string.Empty, pattern) ||
                    EF.Functions.ILike(x.IdentificationNumber ?? string.Empty, pattern) ||
                    EF.Functions.ILike(x.Email                ?? string.Empty, pattern) ||
                    EF.Functions.ILike(x.Phone                ?? string.Empty, pattern) ||
                    EF.Functions.ILike(x.City                 ?? string.Empty, pattern)
                );
        }

        // Ordenamiento
        dbQuery = query.OrderBy?.ToLower() switch
        {
            "firstname"    => query.OrderDescending ? dbQuery.OrderByDescending(x => x.FirstName)    : dbQuery.OrderBy(x => x.FirstName),
            "lastname"     => query.OrderDescending ? dbQuery.OrderByDescending(x => x.LastName)     : dbQuery.OrderBy(x => x.LastName),
            "businessname" => query.OrderDescending ? dbQuery.OrderByDescending(x => x.BusinessName) : dbQuery.OrderBy(x => x.BusinessName),
            "registeredat" => query.OrderDescending ? dbQuery.OrderByDescending(x => x.RegisteredAt) : dbQuery.OrderBy(x => x.RegisteredAt),
            _              => dbQuery.OrderBy(x => x.FirstName ?? x.BusinessName)
        };

        var totalCount = await dbQuery.CountAsync(cancellationToken);

        // Include solo al materializar — después del Count para no afectar rendimiento
        var items = await dbQuery
            .Include(x => x.Supplier)
            .Include(x => x.Member)
            .Include(x => x.Donor)
            .Include(x => x.Employee)
            .Include(x => x.Partner)
            .Include(x => x.Customer)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ThirdPartyEntity>(items, totalCount, query.Page, query.PageSize);
    }

    // ── CREATE ────────────────────────────────────────────────────────────────

    public async Task<ThirdPartyEntity> CreateAsync(ThirdPartyEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.ThirdParties.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    // ── UPDATE ────────────────────────────────────────────────────────────────

    public async Task<ThirdPartyEntity> UpdateAsync(ThirdPartyEntity entity, CancellationToken cancellationToken = default)
    {
        _context.ThirdParties.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    // ── DELETE ────────────────────────────────────────────────────────────────

    public async Task<ThirdPartyEntity> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.ThirdParties
            .Include(x => x.Supplier)
            .Include(x => x.Member)
            .Include(x => x.Donor)
            .Include(x => x.Employee)
            .Include(x => x.Partner)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"ThirdParty con Id {id} no encontrado.");

        _context.ThirdParties.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }
    
}