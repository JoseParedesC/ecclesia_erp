using Ecclesia.Application.AccountingPeriods.DTOs;
using Ecclesia.Application.AccountingPeriods.Queries.ListAccountingPeriods;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common.PagedQuery;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.AccountingPeriods;

public static class List
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", ListAsync)
            .WithName("ListAccountingPeriods")
            .WithSummary("List accounting periods")
            .WithDescription("Returns a paginated list of accounting periods.")
            .RequireAuthorization(EcclesiaPermissions.ACCOUNTING_PERIOD.READ)
            .Produces<PagedResult<AccountingPeriodListDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> ListAsync(
        [AsParameters] PagedQuery query,
        [FromServices] ListAccountingPeriodsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}