using Ecclesia.Application.Rostros.Queries.GetRostroById;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Rostro;

public static class GetById
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", HandleAsync)
            .RequireAuthorization(EcclesiaPermissions.ROSTRO.Read)
            .WithName("GetRostroById")
            .WithSummary("Obtiene el detalle de un Rostro por su Id.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        Guid id,
        [FromServices] GetRostroByIdHandler handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(new GetRostroByIdQuery(id), ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Errors);
    }
}