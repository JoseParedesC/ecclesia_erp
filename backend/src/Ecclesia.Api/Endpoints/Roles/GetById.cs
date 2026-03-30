using Ecclesia.Application.Roles.Queries;
using Microsoft.AspNetCore.Mvc;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common;
using Ecclesia.Application.Roles.DTOs;

namespace Ecclesia.Api.Endpoints.Roles;

public class GetById
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", GetByIdAsync)
        .RequireAuthorization(EcclesiaPermissions.ROLES.READ)
        .WithName("GetRoleById")
        .WithSummary("Get role by ID")
        .WithDescription("Retrieves a role by their ID.")
        .Produces<PagedResult<RoleDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status403Forbidden);
    }

    public static async Task<IResult> GetByIdAsync(
        [FromRoute] Guid id,
        [FromServices] GetRoleByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetRoleByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}