using Ecclesia.Application.Rostros.Commands.DeactivateRostro;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Rostro;

public static class Deactivate
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", HandleAsync)
            .RequireAuthorization(EcclesiaPermissions.ROSTRO.Deactivate)
            .WithName("DeactivateRostro")
            .WithSummary("Desactiva un Rostro (eliminación lógica).")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        Guid id,
        [FromServices] DeactivateRostroHandler handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(new DeactivateRostroCommand(id), ct);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }
}