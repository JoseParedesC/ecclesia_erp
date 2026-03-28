using Ecclesia.Application.Accounts.Commands.CreateAccount;
using Ecclesia.Application.Accounts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Accounts;

public static class Create
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", CreateAsync)
            .WithName("CreateAccount")
            .WithSummary("Create account")
            .WithDescription("Creates a new account.")
            .Produces<AccountDetailDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> CreateAsync(
        [FromBody] CreateAccountDto dto,
        [FromServices] CreateAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand(
            dto.Code,
            dto.Name,
            dto.Type,
            dto.ParentAccountId
        );

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/accounts/{result.Value.Id}", result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}