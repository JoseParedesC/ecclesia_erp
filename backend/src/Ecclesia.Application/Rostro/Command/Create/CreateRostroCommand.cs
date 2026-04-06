namespace Ecclesia.Application.Rostros.Commands.CreateRostro;

public record CreateRostroCommand(
    string  Code,
    string  Name,
    string? Description
);
