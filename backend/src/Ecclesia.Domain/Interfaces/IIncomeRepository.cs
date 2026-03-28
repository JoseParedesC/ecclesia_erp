
using Ecclesia.Domain.Entities.Income;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Common;

namespace Ecclesia.Domain.Interfaces;

public interface IIncomeRepository
{
    Task<IncomeEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<IncomeEntity>> ListAllAsync(PagedQuery query, CancellationToken cancellationToken = default);
    Task AddAsync(IncomeEntity income, CancellationToken cancellationToken = default);
}