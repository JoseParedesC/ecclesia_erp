using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common.PagedQuery;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Permissions;

public static class Catalog
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync)
            .RequireAuthorization(EcclesiaPermissions.ROLES.READ)
            .WithName("ListPermissionss")
            .WithSummary("Lista todos los Permissionss con paginación y filtros opcionales.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync()
    {
        return Results.Ok(EcclesiaPermissions.ToDictionary());
    }
}