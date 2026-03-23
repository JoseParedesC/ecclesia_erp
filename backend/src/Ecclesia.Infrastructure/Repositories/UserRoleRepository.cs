using Ecclesia.Domain.Entities.Roles;
using Ecclesia.Domain.Repositories;
using Ecclesia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecclesia.Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly AppDbContext _context;

    public UserRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
        => await _context.UserRoles
            .AsNoTracking()
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);

    public async Task AddAsync(UserRoleEntity userRole, CancellationToken cancellationToken = default)
    {
        await _context.UserRoles.AddAsync(userRole, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}