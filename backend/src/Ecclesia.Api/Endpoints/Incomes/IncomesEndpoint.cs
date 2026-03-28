namespace Ecclesia.Api.Endpoints.Incomes;

public static class IncomesEndpoint
{
    public static void MapIncomesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/incomes")
            .WithTags("Incomes");

        CreateIncome.Map(group);
    }
}