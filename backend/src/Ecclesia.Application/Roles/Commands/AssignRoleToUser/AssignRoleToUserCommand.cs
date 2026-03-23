namespace Ecclesia.Application.Roles.Commands.AssignRoleToUser;

public record AssignRoleToUserCommand(Guid UserId, Guid RoleId);