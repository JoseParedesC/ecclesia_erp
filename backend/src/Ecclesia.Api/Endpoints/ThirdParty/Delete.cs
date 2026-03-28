using Ecclesia.Application.ThirdParty.Commands.DeleteThirdParty;
using Ecclesia.Application.ThirdParty.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.ThirdParty;

public static class Delete
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", DeleteAsync)
            .WithName("DeleteThirdParty")
            .WithSummary("Delete third party")
            .WithDescription("Deletes a third party by its unique identifier.")
            .Produces<ThirdPartyDetailDto>(StatusCodes.Status200OK)
            .RequireAuthorization(EcclesiaPermissions.THIRD_PARTIES.DELETE)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> DeleteAsync(
        Guid id,
        [FromServices] DeleteThirdPartyHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}