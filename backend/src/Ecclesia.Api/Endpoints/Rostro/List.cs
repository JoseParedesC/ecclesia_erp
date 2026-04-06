using Ecclesia.Application.Rostros.Queries.ListRostros;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common.PagedQuery;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Rostro;

public static class List
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync)
            .RequireAuthorization(EcclesiaPermissions.ROSTRO.Read)
            .WithName("ListRostros")
            .WithSummary("Lista todos los Rostros con paginación y filtros opcionales.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string? search,
        [FromQuery] string? searchField,
        [FromQuery] string? orderBy,
        [FromQuery] bool orderDescending,
        [FromServices] ListRostrosHandler handler,
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