using Ecclesia.Domain.Common.PagedQuery;
using FluentValidation;

namespace Ecclesia.Application.AccountingPeriods.Queries.ListAccountingPeriods;

public class ListAccountingPeriodsValidator : AbstractValidator<PagedQuery>
{
    public ListAccountingPeriodsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page debe ser mayor a 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize debe estar entre 1 y 100.");
    }
}