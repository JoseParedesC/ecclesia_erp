using Ecclesia.Application.Users.DTOs;

namespace Ecclesia.Application.Users.Commands.CreateUser;

public record CreateUserCommand(CreateUserDto userDto);