using FluentValidation;

namespace Ecclesia.Application.Users.Queries;

public class GetAllUsersValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersValidator()
    {
        // No hay validaciones específicas para GetAllUsersQuery
    }
}
