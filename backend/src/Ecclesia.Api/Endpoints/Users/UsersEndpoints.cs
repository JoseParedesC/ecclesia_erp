
using Ecclesia.Api.Endpoints.Roles;

namespace Ecclesia.Api.Endpoints.Users;

public static class UsersEndpoint
{
    public static void MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users");

        GetAll.Map(group);
        GetById.Map(group);
        Create.Map(group); 
        Update.Map(group);
        Delete.Map(group);
        AssignRoleToUser.Map(group);
    }

    
}