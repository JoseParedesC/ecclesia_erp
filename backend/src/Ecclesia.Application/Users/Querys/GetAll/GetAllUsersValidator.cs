using FluentValidation;
using Ecclesia.Domain.Common.PagedQuery;

namespace Ecclesia.Application.Users.Queries;

public class GetAllUsersValidator : AbstractValidator<PagedQuery>
{
    public GetAllUsersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("La página debe ser mayor a 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("El tamaño de página debe ser mayor a 0.")
            .LessThanOrEqualTo(100).WithMessage("El tamaño de página no puede superar 100.");
    }
}
