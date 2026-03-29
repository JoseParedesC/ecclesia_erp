using Ecclesia.Application.AccountingPeriods.Commands.ReopenAccountingPeriod;
using Ecclesia.Application.AccountingPeriods.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.AccountingPeriods;

public static class Reopen
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPatch("/{id:guid}/reopen", ReopenAsync)
            .WithName("ReopenAccountingPeriod")
            .WithSummary("Reopen accounting period")
            .WithDescription("Reopens a closed accounting period. Requires special permission.")
            .RequireAuthorization(EcclesiaPermissions.ACCOUNTING_PERIOD.REOPEN)
            .Produces<AccountingPeriodDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> ReopenAsync(
        Guid id,
        [FromServices] ReopenAccountingPeriodHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ReopenAccountingPeriodCommand(id);
        var result  = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}