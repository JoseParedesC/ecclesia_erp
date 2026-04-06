
namespace Ecclesia.Api.Endpoints.Rostro;

public static class RostrosEndpoint
{
    public static void MapRostrosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/rostros")
            .WithTags("Rostros");

        List.Map(group);
        GetById.Map(group);
        Create.Map(group);
        Update.Map(group);
        Deactivate.Map(group);
    }
}