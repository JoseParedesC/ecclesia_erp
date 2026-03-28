using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities.Account;

namespace Ecclesia.Domain.Repositories;

public interface IAccountRepository
{
    Task<AccountEntity?> GetByIdAsync (Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<AccountEntity>> ListAllAsync (PagedQuery query, CancellationToken cancellationToken = default);
    Task<AccountEntity> CreateAsync (AccountEntity entity, CancellationToken cancellationToken = default);
    Task<AccountEntity> UpdateAsync (AccountEntity entity, CancellationToken cancellationToken = default);
    Task<AccountEntity> DeleteAsync (Guid id, CancellationToken cancellationToken = default);
}