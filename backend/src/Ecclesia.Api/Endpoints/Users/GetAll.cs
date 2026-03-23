using Ecclesia.Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Ecclesia.Api.Endpoints.Users;

public class GetAll
{
    public static async Task<IResult> GetAllAsync(
        [FromServices] GetAllUsersHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAllUsersQuery(), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(new { Errors = result.Errors });
    }
}