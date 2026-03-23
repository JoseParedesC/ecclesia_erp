using Ecclesia.Application.Users.Commands.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Users;

public class Create
{
    public static async Task<IResult> CreateAsync(
        [FromBody] CreateUserCommand command,
        [FromServices] CreateUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/users/{result.Value}", new { Id = result.Value })
            : Results.BadRequest(new { Errors = result.Errors });
    }
}