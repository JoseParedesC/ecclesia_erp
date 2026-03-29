using FluentValidation;

namespace Ecclesia.Application.AccountingPeriods.Queries.GetAccountingPeriodById;

public class GetAccountingPeriodByIdValidator : AbstractValidator<GetAccountingPeriodByIdQuery>
{
    public GetAccountingPeriodByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es requerido.");
    }
}