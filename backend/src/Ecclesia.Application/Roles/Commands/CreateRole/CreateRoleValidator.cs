using FluentValidation;

namespace Ecclesia.Application.Roles.Commands.CreateRole;

public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("El nombre del rol es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(256).WithMessage("La descripción no puede superar 256 caracteres.");
    }
}