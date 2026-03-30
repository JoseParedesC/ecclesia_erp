using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities.Roles;
using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecclesia.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<RoleEntity>> ListAllAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var dbQuery = _context.Roles.AsNoTracking();

        // Filtro de búsqueda
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search}%";

            dbQuery = !string.IsNullOrWhiteSpace(query.SearchField)
                ? query.SearchField.ToLower() switch  // busca solo en el campo especificado
                {
                    "name"     => dbQuery.Where(u => EF.Functions.ILike(u.Name,     pattern)),
                    _          => dbQuery
                }
                : dbQuery.Where(u =>  // busca en todos los campos
                    EF.Functions.ILike(u.Name,     pattern)
                );
        }

        // Ordenamiento — antes del Skip/Take para que opere en BD
        dbQuery = query.OrderBy?.ToLower() switch
        {
            "name"     => query.OrderDescending ? dbQuery.OrderByDescending(u => u.Name)     : dbQuery.OrderBy(u => u.Name),
            _          => dbQuery.OrderBy(u => u.Name) // default
        };

        var totalCount = await dbQuery.CountAsync(cancellationToken);

        var items = await dbQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<RoleEntity>(items, totalCount, query.Page, query.PageSize);
    }

    public async Task<RoleEntity?> GetByIdNoTrackAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);

    public async Task AddAsync(RoleEntity role, CancellationToken cancellationToken = default)
    {
        await _context.Roles.AddAsync(role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}