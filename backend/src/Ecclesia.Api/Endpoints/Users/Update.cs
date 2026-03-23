using Ecclesia.Application.Users.Commands.UpdateUser;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Users;

public class Update
{
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
            ? Results.Ok(new { Id = result.Value })
            : Results.NotFound(new { Errors = result.Errors });
    }
}