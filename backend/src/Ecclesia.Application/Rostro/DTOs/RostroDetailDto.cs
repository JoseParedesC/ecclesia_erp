namespace Ecclesia.Application.Rostros.Queries.GetRostroById;

public record RostroDetailDto(
    Guid     Id,
    string   Code,
    string   Name,
    string?  Description,
    bool     IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
