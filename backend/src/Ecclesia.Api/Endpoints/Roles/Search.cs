using Ecclesia.Application.Roles.DTOs;
using Ecclesia.Application.Roles.Queries.SearchRoles;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Roles;

public static class Search
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/search", SearchAsync)
            .WithName("SearchRoles")
            .WithSummary("Search Roles")
            .WithDescription("Returns a paginated list of Roles matching the search term. Intended for autocomplete inputs.")
            .RequireAuthorization(EcclesiaPermissions.ROLES.READ)
            .Produces<PagedResult<SearchRoleDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> SearchAsync(
        [AsParameters] SearchRoleDto dto,
        [FromServices] SearchRolesHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new SearchRoleQuery(
            dto.Search,
            dto.Page,
            dto.PageSize
        );

        var result = await handler.HandleAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}