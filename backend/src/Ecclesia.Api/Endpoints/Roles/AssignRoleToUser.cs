using Ecclesia.Application.Roles.Commands.AssignRoleToUser;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Roles;

public static class AssignRoleToUser
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/{roleId:guid}/users/{userId:guid}", AssignAsync)
            .RequireAuthorization(EcclesiaPermissions.ROLES.ASSIGN)
            .WithName("AssignRoleToUser")
            .WithSummary("Assign role to user")
            .WithDescription("Assigns an existing role to an existing user.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden);
    }

    public static async Task<IResult> AssignAsync(
        [FromRoute] Guid roleId,
        [FromRoute] Guid userId,
        [FromServices] AssignRoleToUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new AssignRoleToUserCommand(userId, roleId), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(new { Errors = result.Errors });
    }
}