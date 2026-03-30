using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities.Roles;

namespace Ecclesia.Domain.Repositories;

public interface IRoleRepository
{
    Task<PagedResult<RoleEntity>> ListAllAsync(PagedQuery query, CancellationToken cancellationToken = default);
    Task<RoleEntity?> GetByIdNoTrackAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(RoleEntity role, CancellationToken cancellationToken = default);
}