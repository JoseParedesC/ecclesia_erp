using Ecclesia.Application.AccountingPeriods.Commands.CreateAccountingPeriod;
using Ecclesia.Application.AccountingPeriods.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.AccountingPeriods;

public record CreateAccountingPeriodDto(int Year, int Month);

public static class Create
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", CreateAsync)
            .WithName("CreateAccountingPeriod")
            .WithSummary("Create accounting period")
            .WithDescription("Creates a new accounting period.")
            .RequireAuthorization(EcclesiaPermissions.ACCOUNTING_PERIOD.CREATE)
            .Produces<AccountingPeriodDetailDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> CreateAsync(
        [FromBody] CreateAccountingPeriodDto dto,
        [FromServices] CreateAccountingPeriodHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateAccountingPeriodCommand(dto.Year, dto.Month);
        var result  = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/accounting-periods/{result.Value.Id}", result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}