namespace Ecclesia.Api.Endpoints.Accounts;

public static class AccountEndpoint
{
    public static void MapAccountEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/accounts")
            .WithTags("Accounts")
            .RequireAuthorization();

        List.Map(group);
        Create.Map(group);
        GetById.Map(group);
        Update.Map(group);
        Delete.Map(group);
    }
}