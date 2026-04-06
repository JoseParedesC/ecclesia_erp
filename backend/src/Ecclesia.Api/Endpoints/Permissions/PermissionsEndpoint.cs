
namespace Ecclesia.Api.Endpoints.Permissions;

public static class PermissionsEndpoint
{
    public static void MapPermissionsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/permissions")
            .WithTags("Permissions");

        Catalog.Map(group);
    }
}