using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities;

namespace Ecclesia.Domain.Repositories;

public interface IRostroRepository
{
    Task<RostroEntity?>             GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RostroEntity?>             GetByCodeAsync(string code, CancellationToken ct = default);
    Task<bool>                      ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken ct = default);
    Task<bool>                      ExistsByCodeAsync(string code, Guid? excludeId = null, CancellationToken ct = default);
    Task<PagedResult<RostroEntity>> ListAsync(PagedQuery query, CancellationToken ct = default);
    Task                            AddAsync(RostroEntity rostro, CancellationToken ct = default);
    Task                            SaveChangesAsync(CancellationToken ct = default);
}
