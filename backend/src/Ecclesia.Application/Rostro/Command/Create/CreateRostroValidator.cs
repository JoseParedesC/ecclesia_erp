using FluentValidation;

namespace Ecclesia.Application.Rostros.Commands.CreateRostro;

public sealed class CreateRostroValidator : AbstractValidator<CreateRostroCommand>
{
    public CreateRostroValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es obligatorio.")
            .MaximumLength(10).WithMessage("El código no puede superar 10 caracteres.")
            .Matches(@"^[A-Za-z0-9]+$").WithMessage("El código solo puede contener letras y números.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres.")
            .When(x => x.Description is not null);
    }
}
