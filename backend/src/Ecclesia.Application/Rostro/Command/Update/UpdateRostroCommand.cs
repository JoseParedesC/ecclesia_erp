namespace Ecclesia.Application.Rostros.Commands.UpdateRostro;

public record UpdateRostroCommand(
    Guid    Id,
    string  Name,
    string? Description
);
