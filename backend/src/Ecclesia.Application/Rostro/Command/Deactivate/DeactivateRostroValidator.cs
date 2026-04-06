using FluentValidation;

namespace Ecclesia.Application.Rostros.Commands.DeactivateRostro;

public sealed class DeactivateRostroValidator : AbstractValidator<DeactivateRostroCommand>
{
    public DeactivateRostroValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
    }
}
