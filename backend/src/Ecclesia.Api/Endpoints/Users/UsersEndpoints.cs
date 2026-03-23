
namespace Ecclesia.Api.Endpoints.Users;

public static class UsersEndpoint
{
    public static void MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users");

        group.MapGet("/", GetAll.GetAllAsync);
        group.MapGet("/{id:guid}", GetById.GetByIdAsync);
        group.MapPost("/", Create.CreateAsync);
        group.MapPut("/{id:guid}", Update.UpdateAsync);
        group.MapDelete("/{id:guid}", Delete.DeleteAsync);
    }

    
}