namespace Ecclesia.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Name,
    string UserName,
    string Email,
    string Password
);