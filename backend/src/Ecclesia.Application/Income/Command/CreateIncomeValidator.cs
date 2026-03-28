using Ecclesia.Application.Incomes.DTOs;
using FluentValidation;

namespace Ecclesia.Application.Incomes.Commands.CreateIncome;

public class CreateIncomeValidator : AbstractValidator<CreateIncomeDto>
{
    public CreateIncomeValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("La fecha es requerida.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha no puede ser futura.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

        RuleFor(x => x.DonorId)
            .NotEmpty().WithMessage("El donante es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El Id del donante no es válido.");

        RuleFor(x => x.CashAccountId)
            .NotEmpty().WithMessage("La cuenta de caja es requerida.")
            .NotEqual(Guid.Empty).WithMessage("El Id de la cuenta de caja no es válido.");

        RuleFor(x => x.CommunityId)
            .NotEmpty().WithMessage("La comunidad es requerida.")
            .NotEqual(Guid.Empty).WithMessage("El Id de la comunidad no es válido.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es requerida.")
            .MaximumLength(256).WithMessage("La descripción no puede superar 256 caracteres.");
    }
}