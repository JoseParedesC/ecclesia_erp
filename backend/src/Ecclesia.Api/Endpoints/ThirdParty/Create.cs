using Ecclesia.Application.ThirdParty.Commands.CreateThirdParty;
using Ecclesia.Application.ThirdParty.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.ThirdParty;

public static class Create
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", CreateAsync)
            .WithName("CreateThirdParty")
            .WithSummary("Create third party")
            .WithDescription("Creates a new third party.")
            .RequireAuthorization(EcclesiaPermissions.THIRD_PARTIES.CREATE)
            .Produces<ThirdPartyDetailDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> CreateAsync(
        [FromBody] CreateThirdPartyCommand command,
        [FromServices] CreateThirdPartyHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/third-parties/{result.Value.Id}", result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}