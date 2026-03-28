using FluentValidation;

namespace Ecclesia.Application.Accounts.Commands.UpdateAccount;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es requerido.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es requerido.")
            .MaximumLength(20).WithMessage("El código no puede superar 20 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Code));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("El tipo de cuenta no es válido.");
    }
}