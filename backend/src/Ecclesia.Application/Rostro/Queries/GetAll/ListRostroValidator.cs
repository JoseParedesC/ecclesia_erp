using Ecclesia.Domain.Common.PagedQuery;
using FluentValidation;

public class ListRostrosValidator : AbstractValidator<PagedQuery>
{
    public ListRostrosValidator()
    {
        // Page
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("El número de página debe ser mayor a 0.");

        // PageSize
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("El tamaño de página debe ser mayor a 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("El tamaño de página no puede ser mayor a 100.");

        // Search (opcional, pero con límite)
        RuleFor(x => x.Search)
            .MaximumLength(100)
            .WithMessage("El texto de búsqueda no puede exceder los 100 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Search));

        // IsActive → no necesita validación (nullable bool válido)
    }
}