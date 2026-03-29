using FluentValidation;

namespace Ecclesia.Application.AccountingPeriods.Commands.ReopenAccountingPeriod;

public class ReopenAccountingPeriodValidator : AbstractValidator<ReopenAccountingPeriodCommand>
{
    public ReopenAccountingPeriodValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es requerido.");
    }
}