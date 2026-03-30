using Microsoft.AspNetCore.Mvc;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Application.Roles.DTOs;
using Ecclesia.Application.Roles.Queries.GetAllRoles;

namespace Ecclesia.Api.Endpoints.Roles;

public class GetAll
{

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllAsync)
        .RequireAuthorization(EcclesiaPermissions.ROLES.READ)
        .WithName("GetAllRoles")
        .WithSummary("Get all roles")
        .WithDescription("Retrieves all roles in the system.")
        .Produces<PagedResult<RoleDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status403Forbidden);
    }
    
    public static async Task<IResult> GetAllAsync(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string? search,
        [FromQuery] string? searchField,
        [FromQuery] string? orderBy,
        [FromQuery] bool orderDescending,
        [FromServices] GetAllRolesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            new PagedQuery
            {
                Page = page, 
                PageSize = pageSize, 
                Search = search, 
                SearchField = searchField, 
                OrderBy = orderBy, 
                OrderDescending = orderDescending
            },
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}