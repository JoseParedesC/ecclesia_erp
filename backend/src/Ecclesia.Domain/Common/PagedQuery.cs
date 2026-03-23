namespace Ecclesia.Domain.Common.PagedQuery;

public record PagedQuery
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Search { get; init; }
    public string? SearchField { get; init; }
    public string? OrderBy { get; init; }
    public bool OrderDescending { get; init; } = false;
}