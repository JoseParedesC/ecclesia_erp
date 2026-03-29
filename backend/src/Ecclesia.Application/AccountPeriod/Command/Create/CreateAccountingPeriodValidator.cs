using Ecclesia.Domain.Common.Enums;
using FluentValidation;

namespace Ecclesia.Application.AccountingPeriod.Commands.CreateAccountingPeriod;

public class CreateAccountingPeriodValidator : AbstractValidator<CreateAccountingPeriodCommand>
{
    public CreateAccountingPeriodValidator()
    {
        RuleFor(x => x.Period)
            .NotNull().WithMessage("El período nuevo es requerido.");

        // RuleFor(x => x.Oldperiod)
        //     .NotNull().WithMessage("El período antiguo es requerido.");

    }
}