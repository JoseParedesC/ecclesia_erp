using Ecclesia.Application.Users.Commands.DeleteUser;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Users;

public class Delete
{
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