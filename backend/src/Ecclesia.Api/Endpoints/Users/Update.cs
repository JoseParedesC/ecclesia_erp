using Ecclesia.Application.Users.Commands.UpdateUser;
using Microsoft.AspNetCore.Mvc;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common;
using Ecclesia.Application.Users.DTOs;
namespace Ecclesia.Api.Endpoints.Users;

public class Update
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", UpdateAsync)
        .RequireAuthorization(EcclesiaPermissions.USER.UPDATE)
        .WithName("UpdateUser")
        .WithSummary("Update user")
        .WithDescription("Updates a user in the system.")
        .Produces<PagedResult<UserDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status403Forbidden);
    }

    public static async Task<IResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserCommand command,
        [FromServices] UpdateUserHandler handler,
        CancellationToken cancellationToken)
    {
        // Asegura que el Id de la ruta coincida con el comando
        if (id != command.Id)
            return Results.BadRequest(new { Errors = new[] { "El Id de la ruta no coincide con el Id del comando." } });

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.NotFound(new { Errors = result.Errors });
    }
}