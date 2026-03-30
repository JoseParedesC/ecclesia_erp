namespace Ecclesia.Api.Endpoints.Roles;

public static class RolesEndpoint
{
    public static void MapRolesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/roles")
            .WithTags("Roles");

        CreateRole.Map(group);
        GetAll.Map(group);
        GetById.Map(group);
        Search.Map(group);
    }
}