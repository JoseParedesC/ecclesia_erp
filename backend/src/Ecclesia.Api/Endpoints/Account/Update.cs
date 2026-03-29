using Ecclesia.Application.Accounts.Commands.UpdateAccount;
using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Accounts;

public static class Update
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", UpdateAsync)
            .WithName("UpdateAccount")
            .WithSummary("Update account")
            .WithDescription("Updates an existing account.")
            .RequireAuthorization(EcclesiaPermissions.ACCOUNT.UPDATE)
            .Produces<AccountDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> UpdateAsync(
        Guid id,
        [FromBody] UpdateAccountDto dto,
        [FromServices] UpdateAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAccountCommand(
            id,
            dto.Code,
            dto.Name,
            dto.Type,
            dto.ParentAccountId
        );

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}