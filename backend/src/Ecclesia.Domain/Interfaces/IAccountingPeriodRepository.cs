using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities.AccountingPeriod;

namespace Ecclesia.Domain.Repositories;

public interface IAccountingPeriodRepository
{
    Task<AccountingPeriodEntity?> GetByIdAsync (Guid id, CancellationToken cancellationToken = default);
    Task<AccountingPeriodEntity?> GetByYearMonthAsync(int year, int month, CancellationToken cancellationToken = default);
    Task<AccountingPeriodEntity?> GetCurrentOpenAsync( CancellationToken cancellationToken = default);
    Task<PagedResult<AccountingPeriodEntity>> ListAllAsync (PagedQuery query, CancellationToken cancellationToken = default); 
    Task<AccountingPeriodEntity> CreateAsync (AccountingPeriodEntity entity, CancellationToken cancellationToken = default);
    Task<AccountingPeriodEntity> UpdateAsync (AccountingPeriodEntity entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsByYearMonthAsync(int year, int month, CancellationToken cancellationToken = default);
    // Task<AccountingPeriodEntity?> GetOpenPeriodAsync(DateTime period, CancellationToken cancellationToken = default);
}