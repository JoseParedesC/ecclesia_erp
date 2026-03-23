using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.Roles;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Roles.Commands.AssignRoleToUser;

public class AssignRoleToUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly AssignRoleToUserValidator _validator;

    public AssignRoleToUserHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        AssignRoleToUserValidator validator)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _validator = validator;
    }

    public async Task<Result> HandleAsync(AssignRoleToUserCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result.Failure(errors);
        }

        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return Result.Failure($"Usuario con Id '{command.UserId}' no encontrado.");

        var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
        if (role is null)
            return Result.Failure($"Rol con Id '{command.RoleId}' no encontrado.");

        var exists = await _userRoleRepository.ExistsAsync(command.UserId, command.RoleId, cancellationToken);
        if (exists)
            return Result.Failure($"El usuario ya tiene asignado el rol '{role.Name}'.");

        var userRole = new UserRoleEntity
        {
            UserId = command.UserId,
            RoleId = command.RoleId
        };

        await _userRoleRepository.AddAsync(userRole, cancellationToken);

        return Result.Success();
    }
}