using FluentValidation;

namespace Ecclesia.Application.Roles.Queries;

public class GetRoleByIdValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("El Id del usuario es requerido.")
            .NotEqual(Guid.Empty)
            .WithMessage("El Id del usuario no es válido.");
    }
}