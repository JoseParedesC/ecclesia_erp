using FluentValidation;

namespace Ecclesia.Application.Rostros.Commands.UpdateRostro;

public sealed class UpdateRostroValidator : AbstractValidator<UpdateRostroCommand>
{
    public UpdateRostroValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres.")
            .When(x => x.Description is not null);
    }
}
