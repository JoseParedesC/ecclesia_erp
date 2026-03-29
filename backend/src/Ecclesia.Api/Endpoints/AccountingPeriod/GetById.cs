using Ecclesia.Application.AccountingPeriods.DTOs;
using Ecclesia.Application.AccountingPeriods.Queries.GetAccountingPeriodById;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.AccountingPeriods;

public static class GetById
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", GetByIdAsync)
            .WithName("GetAccountingPeriodById")
            .WithSummary("Get accounting period by id")
            .WithDescription("Returns a single accounting period by its unique identifier.")
            .RequireAuthorization(EcclesiaPermissions.ACCOUNTING_PERIOD.READ)
            .Produces<AccountingPeriodDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> GetByIdAsync(
        Guid id,
        [FromServices] GetAccountingPeriodByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}