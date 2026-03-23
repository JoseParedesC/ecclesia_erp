using FluentValidation;

namespace Ecclesia.Application.Incomes.Commands.CreateIncome;

public class CreateIncomeValidator : AbstractValidator<CreateIncomeCommand>
{
    public CreateIncomeValidator()
    {
        RuleFor(x => x.Dto.Date)
            .NotEmpty().WithMessage("La fecha es requerida.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La fecha no puede ser futura.");

        RuleFor(x => x.Dto.Amount)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

        RuleFor(x => x.Dto.DonorId)
            .NotEmpty().WithMessage("El donante es requerido.")
            .NotEqual(Guid.Empty).WithMessage("El Id del donante no es válido.");

        RuleFor(x => x.Dto.CashAccountId)
            .NotEmpty().WithMessage("La cuenta de caja es requerida.")
            .NotEqual(Guid.Empty).WithMessage("El Id de la cuenta de caja no es válido.");

        RuleFor(x => x.Dto.CommunityId)
            .NotEmpty().WithMessage("La comunidad es requerida.")
            .NotEqual(Guid.Empty).WithMessage("El Id de la comunidad no es válido.");

        RuleFor(x => x.Dto.Description)
            .NotEmpty().WithMessage("La descripción es requerida.")
            .MaximumLength(256).WithMessage("La descripción no puede superar 256 caracteres.");
    }
}