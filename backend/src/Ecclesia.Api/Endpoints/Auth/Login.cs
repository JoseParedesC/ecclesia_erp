using Ecclesia.Application.Auth.Commands.Login;
using Ecclesia.Application.Auth.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Auth;

public static class Login
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Login")
            .WithDescription("Authenticates a user and returns a JWT token.")
            .Produces<AuthResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous();
    }

    public static async Task<IResult> LoginAsync(
        [FromBody] LoginDto dto,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new LoginCommand(dto), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}