using Ecclesia.Application.Users.Commands.DeleteUser;
using Microsoft.AspNetCore.Mvc;
using Ecclesia.Application.Users.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common;

namespace Ecclesia.Api.Endpoints.Users;

public class Delete
{

    public static void Map(RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", DeleteAsync)
        .RequireAuthorization(EcclesiaPermissions.USER.DELETE)
        .WithName("DeleteUser")
        .WithSummary("Delete user")
        .WithDescription("Deletes a user from the system.")
        .Produces<PagedResult<UserSummaryDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status403Forbidden);
    }

    public static async Task<IResult> DeleteAsync(
        [FromRoute] Guid id,
        [FromServices] DeleteUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new DeleteUserCommand(id), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.NotFound(new { Errors = result.Errors });
    }
}