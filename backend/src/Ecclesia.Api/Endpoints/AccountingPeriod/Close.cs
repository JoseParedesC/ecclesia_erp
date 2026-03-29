using Ecclesia.Application.AccountingPeriods.Commands.CloseAccountingPeriod;
using Ecclesia.Application.AccountingPeriods.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.AccountingPeriods;

public static class Close
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPatch("/{id:guid}/close", CloseAsync)
            .WithName("CloseAccountingPeriod")
            .WithSummary("Close accounting period")
            .WithDescription("Closes an open accounting period.")
            .RequireAuthorization(EcclesiaPermissions.ACCOUNTING_PERIOD.CLOSE)
            .Produces<AccountingPeriodDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> CloseAsync(
        Guid id,
        [FromServices] CloseAccountingPeriodHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CloseAccountingPeriodCommand(id);
        var result  = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}