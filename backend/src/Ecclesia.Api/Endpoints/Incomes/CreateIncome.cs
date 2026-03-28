using Ecclesia.Application.Incomes.Commands.CreateIncome;
using Ecclesia.Application.Incomes.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Incomes;

public static class CreateIncome
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", CreateAsync)
            .RequireAuthorization(EcclesiaPermissions.USER.CREATE)
            .WithName("CreateIncome")
            .WithSummary("Create income")
            .WithDescription("Creates a new income and generates a journal voucher automatically.")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status403Forbidden);
    }

    public static async Task<IResult> CreateAsync(
        [FromBody] CreateIncomeDto requestBody,
        [FromServices] CreateIncomeHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(requestBody, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/incomes/{result.Value}", new { Id = result.Value })
            : Results.BadRequest(new { Errors = result.Errors });
    }
}