namespace Ecclesia.Application.Rostros.Queries.ListRostros;

public record RostroSummaryDto(
    Guid   Id,
    string Code,
    string Name,
    bool   IsActive
);
