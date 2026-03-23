namespace Ecclesia.Api.Endpoints.Auth;

public static class AuthEndpoint
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

        Login.Map(group);
        Me.Map(group);
    }
}