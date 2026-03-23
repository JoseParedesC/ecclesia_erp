using Ecclesia.Domain.Entities.Users;
using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;

namespace Ecclesia.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<UserEntity>> ListAllAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var dbQuery = _context.Users.AsNoTracking();

        // Filtro de búsqueda
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search}%";

            dbQuery = !string.IsNullOrWhiteSpace(query.SearchField)
                ? query.SearchField.ToLower() switch  // busca solo en el campo especificado
                {
                    "name"     => dbQuery.Where(u => EF.Functions.ILike(u.Name,     pattern)),
                    "email"    => dbQuery.Where(u => EF.Functions.ILike(u.Email,    pattern)),
                    "username" => dbQuery.Where(u => EF.Functions.ILike(u.UserName, pattern)),
                    _          => dbQuery
                }
                : dbQuery.Where(u =>  // busca en todos los campos
                    EF.Functions.ILike(u.Name,     pattern) ||
                    EF.Functions.ILike(u.Email,    pattern) ||
                    EF.Functions.ILike(u.UserName, pattern)
                );
        }

        // Ordenamiento — antes del Skip/Take para que opere en BD
        dbQuery = query.OrderBy?.ToLower() switch
        {
            "name"     => query.OrderDescending ? dbQuery.OrderByDescending(u => u.Name)     : dbQuery.OrderBy(u => u.Name),
            "email"    => query.OrderDescending ? dbQuery.OrderByDescending(u => u.Email)    : dbQuery.OrderBy(u => u.Email),
            "username" => query.OrderDescending ? dbQuery.OrderByDescending(u => u.UserName) : dbQuery.OrderBy(u => u.UserName),
            _          => dbQuery.OrderBy(u => u.Name) // default
        };

        var totalCount = await dbQuery.CountAsync(cancellationToken);

        var items = await dbQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<UserEntity>(items, totalCount, query.Page, query.PageSize);
    }

    public async Task<UserEntity?> GetByIdNoTrackAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}