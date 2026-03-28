using Ecclesia.Domain.Entities.AccountingPeriod;

public interface IAccountingPeriodRepository
{
    Task<AccountingPeriodEntity?> GetOpenPeriodAsync(Guid communityId, CancellationToken cancellationToken = default);
    Task<Guid> GetOpenPeriodIdAsync(DateTime date, CancellationToken ct);
}