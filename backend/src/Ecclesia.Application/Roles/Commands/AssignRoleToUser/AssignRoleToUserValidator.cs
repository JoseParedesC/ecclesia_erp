using FluentValidation;

namespace Ecclesia.Application.Roles.Commands.AssignRoleToUser;

public class AssignRoleToUserValidator : AbstractValidator<AssignRoleToUserCommand>
{
    public AssignRoleToUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El Id del usuario es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El Id del usuario no es válido.");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("El Id del rol es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El Id del rol no es válido.");
    }
}