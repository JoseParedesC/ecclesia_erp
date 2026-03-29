namespace Ecclesia.Api.Endpoints.AccountingPeriods;

public static class AccountingPeriodEndpoint
{
    public static void MapAccountingPeriodEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/accounting-periods")
            .WithTags("AccountingPeriods")
            .RequireAuthorization();

        List.Map(group);
        GetById.Map(group);
        Create.Map(group);
        Close.Map(group);
        Reopen.Map(group);
    }
}