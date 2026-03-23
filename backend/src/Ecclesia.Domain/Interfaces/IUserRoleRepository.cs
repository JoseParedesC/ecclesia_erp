using Ecclesia.Domain.Entities.Roles;

namespace Ecclesia.Domain.Repositories;

public interface IUserRoleRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
    Task AddAsync(UserRoleEntity userRole, CancellationToken cancellationToken = default);
}