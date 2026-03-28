namespace Ecclesia.Api.Endpoints.ThirdParty;

public static class ThirdPartyEndpoint
{
    public static void MapThirdPartyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/third-parties")
            .WithTags("ThirdParty")
            .RequireAuthorization();

        List.Map(group);
        GetById.Map(group);
        Create.Map(group);
        Update.Map(group);
        Delete.Map(group);
    }
}