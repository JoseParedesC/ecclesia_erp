using Ecclesia.Application.Accounts.Commands.DeleteAccount;
using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Domain.Common.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Accounts;

public static class Delete
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", DeleteAsync)
            .WithName("DeleteAccount")
            .WithSummary("Delete account")
            .WithDescription("Deletes an account by its unique identifier.")
            .RequireAuthorization(EcclesiaPermissions.ACCOUNT.DELETE)
            .Produces<AccountDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> DeleteAsync(
        Guid id,
        [FromServices] DeleteAccountHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}