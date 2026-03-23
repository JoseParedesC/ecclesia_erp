using FluentValidation;

namespace Ecclesia.Application.Users.Queries;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("El Id del usuario es requerido.")
            .NotEqual(Guid.Empty)
            .WithMessage("El Id del usuario no es válido.");
    }
}