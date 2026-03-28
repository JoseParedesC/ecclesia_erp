using Ecclesia.Domain.Common.Enums;
using FluentValidation;

namespace Ecclesia.Application.ThirdParty.Commands.CreateThirdParty;

public class CreateThirdPartyValidator : AbstractValidator<CreateThirdPartyCommand>
{
    public CreateThirdPartyValidator()
    {
        RuleFor(x => x.TypeIden)
            .IsInEnum().WithMessage("Tipo de identificación no válido.");

        RuleFor(x => x.PersonType)
            .IsInEnum().WithMessage("Tipo de persona no válido.");

        // Persona Natural
        When(x => x.PersonType == PersonType.Natural, () =>
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es requerido para persona natural.")
                .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido para persona natural.")
                .MaximumLength(100).WithMessage("El apellido no puede superar 100 caracteres.");
        });

        // Persona Jurídica
        When(x => x.PersonType == PersonType.Juridical, () =>
        {
            RuleFor(x => x.BusinessName)
                .NotEmpty().WithMessage("La razón social es requerida para persona jurídica.")
                .MaximumLength(200).WithMessage("La razón social no puede superar 200 caracteres.");
        });

        RuleFor(x => x.IdentificationNumber)
            .NotEmpty().WithMessage("El número de identificación es requerido.")
            .MaximumLength(20).WithMessage("El número de identificación no puede superar 20 caracteres.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("El email no es válido.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Address)
            .MaximumLength(300).WithMessage("La dirección no puede superar 300 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));
    }
}