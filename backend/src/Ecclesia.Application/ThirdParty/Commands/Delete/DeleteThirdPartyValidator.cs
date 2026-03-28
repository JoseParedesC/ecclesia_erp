using FluentValidation;

namespace Ecclesia.Application.ThirdParty.Commands.DeleteThirdParty;

public class DeleteThirdPartyValidator : AbstractValidator<DeleteThirdPartyCommand>
{
    public DeleteThirdPartyValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es requerido.");
    }
}