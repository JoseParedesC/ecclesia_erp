using Ecclesia.Domain.Entities.Roles;
using Ecclesia.Domain.Entities.Users;
using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecclesia.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    => await _context.UserRoles
        .AsNoTracking()
        .Include(ur => ur.Role)
        .ThenInclude(r => r.Permissions)
        .Where(ur => ur.UserId == userId)
        .SelectMany(ur => ur.Role.Permissions)
        .Where(p => p.Schema != null && p.Option != null && p.Permission != null)
        .Select(p => $"{p.Schema}.{p.Option}.{p.Permission}")
        .Distinct()
        .ToListAsync(cancellationToken);

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .ToListAsync(cancellationToken);
}