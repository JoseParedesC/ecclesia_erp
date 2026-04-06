using Ecclesia.Api.Endpoints.Rostro;
using Ecclesia.Application.Rostros.Commands.UpdateRostro;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Rostro;

public static class Update
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", HandleAsync)
            .RequireAuthorization(EcclesiaPermissions.ROSTRO.Update)
            .WithName("UpdateRostro")
            .WithSummary("Actualiza nombre y descripción de un Rostro activo.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        Guid id,
        [FromBody] UpdateRostroCommand body,
        [FromServices] UpdateRostroHandler handler,
        CancellationToken ct)
    {
        var command = new UpdateRostroCommand(id, body.Name, body.Description);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.Ok(new { id = result.Value })
            : Results.BadRequest(result.Errors);
    }
}