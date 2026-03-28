using FluentValidation;

namespace Ecclesia.Application.ThirdParty.Commands.UpdateThirdParty;

public class UpdateThirdPartyValidator : AbstractValidator<UpdateThirdPartyCommand>
{
    public UpdateThirdPartyValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es requerido.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("El email no es válido.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.FirstName)
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage("El apellido no puede superar 100 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.BusinessName)
            .MaximumLength(200).WithMessage("La razón social no puede superar 200 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.BusinessName));

        RuleFor(x => x.Address)
            .MaximumLength(300).WithMessage("La dirección no puede superar 300 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));
    }
}