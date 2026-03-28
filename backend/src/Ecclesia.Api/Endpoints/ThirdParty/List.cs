using Ecclesia.Application.ThirdParty.DTOs;
using Ecclesia.Application.ThirdParty.Queries.ListThirdParties;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.Constants.Permissions;
using Ecclesia.Domain.Common.PagedQuery;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.ThirdParty;

public static class List
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", ListAsync)
            .WithName("ListThirdParties")
            .WithSummary("List third parties")
            .WithDescription("Returns a paginated list of third parties.")
            .RequireAuthorization(EcclesiaPermissions.THIRD_PARTIES.READ)
            .Produces<PagedResult<ThirdPartyListDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    public static async Task<IResult> ListAsync(
        [AsParameters] PagedQuery query,
        [FromServices] ListThirdPartiesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(new { Errors = result.Errors });
    }
}