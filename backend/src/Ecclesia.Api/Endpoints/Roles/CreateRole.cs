using Ecclesia.Application.Roles.Commands.CreateRole;
using Ecclesia.Application.Roles.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Roles;

public static class CreateRole
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", CreateAsync)
            .RequireAuthorization(EcclesiaPermissions.ROLES.CREATE)
            .WithName("CreateRole")
            .WithSummary("Create a role")
            .WithDescription("Creates a new role in the system.")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden);
    }

    public static async Task<IResult> CreateAsync(
        [FromBody] CreateRoleDto dto,
        [FromServices] CreateRoleHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new CreateRoleCommand(dto), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/roles/{result.Value}", new { Id = result.Value })
            : Results.BadRequest(new { Errors = result.Errors });
    }
}