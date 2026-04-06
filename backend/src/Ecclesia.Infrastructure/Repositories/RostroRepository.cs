using Ecclesia.Application.Rostros.Queries.ListRostros;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities;
using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecclesia.Infrastructure.Repositories;

public sealed class RostroRepository(AppDbContext context) : IRostroRepository
{
    // NOTA: GetByIdAsync no usa AsNoTracking para que los handlers de escritura
    // (UpdateRostroHandler, DeactivateRostroHandler) puedan mutar y guardar
    // la entidad a través del mismo DbContext.
    public async Task<RostroEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Rostros
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<RostroEntity?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await context.Rostros
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Code == code.ToUpperInvariant(), ct);

    public async Task<bool> ExistsByNameAsync(
        string name,
        Guid? excludeId = null,
        CancellationToken ct = default)
        => await context.Rostros
            .AsNoTracking()
            .AnyAsync(r =>
                EF.Functions.ILike(r.Name, name) &&
                (excludeId == null || r.Id != excludeId), ct);

    public async Task<bool> ExistsByCodeAsync(
        string code,
        Guid? excludeId = null,
        CancellationToken ct = default)
        => await context.Rostros
            .AsNoTracking()
            .AnyAsync(r =>
                r.Code == code.ToUpperInvariant() &&
                (excludeId == null || r.Id != excludeId), ct);

    public async Task<PagedResult<RostroEntity>> ListAsync(PagedQuery query, CancellationToken CancellationToken = default)
    {
        var dbQuery = context.Rostros.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search}%";

            dbQuery = !string.IsNullOrWhiteSpace(query.SearchField)
                ? query.SearchField.ToLower() switch
                {
                    "name" => dbQuery.Where(r => EF.Functions.ILike(r.Name, pattern)),
                    "code" => dbQuery.Where(r => EF.Functions.ILike(r.Code, pattern)),
                    _      => dbQuery
                }
                : dbQuery.Where(r =>
                    EF.Functions.ILike(r.Name, pattern) ||
                    EF.Functions.ILike(r.Code, pattern));
        }

        // // Fitlro adicional (propio de Rostros)
        // if (query is ListRostrosQuery rostrosQuery && rostrosQuery.IsActive.HasValue)
        // {
        //     dbQuery = dbQuery.Where(r => r.IsActive == rostrosQuery.IsActive.Value);
        // }

        dbQuery = query.OrderBy?.ToLower() switch
        {
            "name" => query.OrderDescending
                ? dbQuery.OrderByDescending(r => r.Name)
                : dbQuery.OrderBy(r => r.Name),

            "code" => query.OrderDescending
                ? dbQuery.OrderByDescending(r => r.Code)
                : dbQuery.OrderBy(r => r.Code),

            _ => dbQuery.OrderBy(r => r.Name) // default
        };

        var totalCount = await dbQuery.CountAsync(CancellationToken);

        var items = await dbQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(CancellationToken);

        return new PagedResult<RostroEntity>(
            items,
            totalCount,
            query.Page,
            query.PageSize);
    }

    public async Task AddAsync(RostroEntity rostro, CancellationToken ct = default)
        => await context.Rostros.AddAsync(rostro, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
