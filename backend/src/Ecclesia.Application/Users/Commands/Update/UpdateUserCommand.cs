using Ecclesia.Application.Users.DTOs;
using Ecclesia.Domain.Common;
using MediatR;
namespace Ecclesia.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, UpdateUserDto userDto) : IRequest<Result>
{
};