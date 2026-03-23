using Ecclesia.Domain.Entities.Roles;

namespace Ecclesia.Domain.Repositories;

public interface IRoleRepository
{
    Task<RoleEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(RoleEntity role, CancellationToken cancellationToken = default);
}