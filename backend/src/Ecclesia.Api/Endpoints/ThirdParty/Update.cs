using Ecclesia.Application.ThirdParty.Commands.UpdateThirdParty;
using Ecclesia.Application.ThirdParty.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.ThirdParty;

public static class Update
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", UpdateAsync)
            .WithName("UpdateThirdParty")
            .WithSummary("Update third party")
            .WithDescription("Updates an existing third party.")
            .RequireAuthorization(EcclesiaPermissions.THIRD_PARTIES.UPDATE)
            .Produces<ThirdPartyDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> UpdateAsync(
        Guid id,
        [FromBody] UpdateThirdPartyCommand command,
        [FromServices] UpdateThirdPartyHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command with { Id = id }, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}