using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Application.Accounts.Queries.GetAccountById;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Accounts;

public static class GetById
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", GetByIdAsync)
            .WithName("GetAccountById")
            .WithSummary("Get account by id")
            .WithDescription("Returns a single account by its unique identifier.")
            .Produces<AccountDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> GetByIdAsync(
        Guid id,
        [FromServices] GetAccountByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}