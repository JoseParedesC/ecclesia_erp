using FluentValidation;

namespace Ecclesia.Application.Accounts.Queries.SearchAccounts;

public class SearchAccountValidator : AbstractValidator<SearchAccountQuery>
{
    public SearchAccountValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page debe ser mayor a 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 20).WithMessage("PageSize debe estar entre 1 y 20.");
    }
}