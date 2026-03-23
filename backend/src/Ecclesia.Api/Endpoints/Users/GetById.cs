using Ecclesia.Application.Users.DTOs;
using Ecclesia.Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common;

namespace Ecclesia.Api.Endpoints.Users;

public class GetById
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", GetByIdAsync)
        .RequireAuthorization(EcclesiaPermissions.USER.READ)
        .WithName("GetUserById")
        .WithSummary("Get user by ID")
        .WithDescription("Retrieves a user by their ID.")
        .Produces<PagedResult<UserSummaryDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status403Forbidden);
    }

    public static async Task<IResult> GetByIdAsync(
        [FromRoute] Guid id,
        [FromServices] GetUserByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetUserByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}