using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Application.Accounts.Queries.SearchAccounts;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Accounts;

public static class Search
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/search", SearchAsync)
            .WithName("SearchAccounts")
            .WithSummary("Search accounts")
            .WithDescription("Returns a paginated list of accounts matching the search term. Intended for autocomplete inputs.")
            .RequireAuthorization(EcclesiaPermissions.ACCOUNT.READ)
            .Produces<PagedResult<AccountSearchDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> SearchAsync(
        [AsParameters] SearchAccountDto dto,
        [FromServices] SearchAccountsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new SearchAccountQuery(
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