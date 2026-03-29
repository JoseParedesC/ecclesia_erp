using FluentValidation;

namespace Ecclesia.Application.AccountingPeriods.Commands.CloseAccountingPeriod;

public class CloseAccountingPeriodValidator : AbstractValidator<CloseAccountingPeriodCommand>
{
    public CloseAccountingPeriodValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es requerido.");

    }
}