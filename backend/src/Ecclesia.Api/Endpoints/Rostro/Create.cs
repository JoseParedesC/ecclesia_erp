using Ecclesia.Application.Rostros.Commands.CreateRostro;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Rostro;

public static class Create
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync)
            .RequireAuthorization(EcclesiaPermissions.ROSTRO.Create)
            .WithName("CreateRostro")
            .WithSummary("Crea un nuevo Rostro.")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] CreateRostroCommand command,
        [FromServices] CreateRostroHandler handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.Created($"/api/rostros/{result.Value}", new { id = result.Value })
            : Results.Conflict(result.Errors);
    }
}