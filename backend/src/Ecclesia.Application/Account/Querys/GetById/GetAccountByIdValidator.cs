using FluentValidation;

namespace Ecclesia.Application.Accounts.Queries.GetAccountById;

public class GetAccountByIdValidator : AbstractValidator<GetAccountByIdQuery>
{
    public GetAccountByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es requerido.");
    }
}