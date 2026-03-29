using Ecclesia.Domain.Entities.AccountingPeriod;

public interface IAccountingPeriodRepository
{
    Task<AccountingPeriodEntity?> GetOpenPeriodAsync(Guid communityId, CancellationToken cancellationToken = default);
    Task<AccountingPeriodEntity?> GetPeriodAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<Guid> GetOpenPeriodIdAsync(DateTime date, CancellationToken ct);
    Task<AccountingPeriodEntity> CreateOpenPeriodAsync(AccountingPeriodEntity newPeriod, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsyn(CancellationToken cancellationToken);
}