public interface IAccountingPeriodService
{
    Task<Guid> GetOpenPeriodIdAsync(DateTime date, CancellationToken ct);
}