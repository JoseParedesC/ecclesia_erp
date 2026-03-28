using FluentValidation;

namespace Ecclesia.Application.ThirdParty.Queries.GetThirdPartyById;

public class GetThirdPartyByIdValidator : AbstractValidator<GetThirdPartyByIdQuery>
{
    public GetThirdPartyByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es requerido.");
    }
}