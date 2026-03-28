using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities.ThirdParty;

namespace Ecclesia.Domain.Repositories;

public interface IThirdPartyRepository
{
    Task<ThirdPartyEntity?> GetByIdAsync (Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<ThirdPartyEntity>> ListAllAsync (PagedQuery query, CancellationToken cancellationToken = default);
    Task<ThirdPartyEntity> CreateAsync (ThirdPartyEntity entity, CancellationToken cancellationToken = default);
    Task<ThirdPartyEntity> UpdateAsync (ThirdPartyEntity entity, CancellationToken cancellationToken = default);
    Task<ThirdPartyEntity> DeleteAsync (Guid id, CancellationToken cancellationToken = default);
}