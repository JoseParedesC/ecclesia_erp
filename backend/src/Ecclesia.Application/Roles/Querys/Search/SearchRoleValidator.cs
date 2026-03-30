using FluentValidation;

namespace Ecclesia.Application.Roles.Queries.SearchRoles;

public class SearchRoleValidator : AbstractValidator<SearchRoleQuery>
{
    public SearchRoleValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page debe ser mayor a 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 20).WithMessage("PageSize debe estar entre 1 y 20.");
    }
}