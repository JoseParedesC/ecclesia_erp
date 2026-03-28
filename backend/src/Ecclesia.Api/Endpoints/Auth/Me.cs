using Ecclesia.Application.Auth.DTOs;
using Ecclesia.Application.Auth.Queries.Me;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Auth;

public static class Me
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/me", MeAsync)
            .RequireAuthorization()
            .WithName("Me")
            .WithSummary("Get current user")
            .WithDescription("Returns the authenticated user's profile, roles and permissions.")
            .Produces<MeDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> MeAsync(
        [FromServices] MeHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}