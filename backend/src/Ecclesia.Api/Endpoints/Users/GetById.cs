using Ecclesia.Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Users;

public class GetById
{
    public static async Task<IResult> GetByIdAsync(
        [FromRoute] Guid id,
        [FromServices] GetUserByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetUserByIdQuery(id), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}