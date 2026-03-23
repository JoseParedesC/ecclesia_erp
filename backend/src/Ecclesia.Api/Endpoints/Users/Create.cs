using Ecclesia.Application.Users.Commands.CreateUser;
using Microsoft.AspNetCore.Mvc;
using Ecclesia.Application.Users.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common;

namespace Ecclesia.Api.Endpoints.Users;

public class Create
{

    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", CreateAsync)
        .RequireAuthorization(EcclesiaPermissions.USER.CREATE)
        .WithName("CreateUser")
        .WithSummary("Create user")
        .WithDescription("Creates a new user in the system.")
        .Produces<PagedResult<UserSummaryDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status403Forbidden);
    }

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