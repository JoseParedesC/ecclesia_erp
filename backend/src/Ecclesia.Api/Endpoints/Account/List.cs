using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Application.Accounts.Queries.ListAccounts;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Accounts;

public static class List
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", ListAsync)
            .WithName("ListAccounts")
            .WithSummary("List accounts")
            .WithDescription("Returns a paginated list of accounts.")
            .Produces<PagedResult<AccountListDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> ListAsync(
        [AsParameters] PagedQuery query,
        [FromServices] ListAccountsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}