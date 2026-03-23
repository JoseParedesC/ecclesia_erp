using Ecclesia.Application.Roles.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Roles.Commands.CreateRole;

public class CreateRoleHandler
{
    private readonly IRoleRepository _roleRepository;
    private readonly CreateRoleValidator _validator;

    public CreateRoleHandler(IRoleRepository roleRepository, CreateRoleValidator validator)
    {
        _roleRepository = roleRepository;
        _validator = validator;
    }

    public async Task<Result<Guid>> HandleAsync(CreateRoleCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<Guid>.Failure(errors);
        }

        var existingRole = await _roleRepository.GetByNameAsync(command.Dto.Name, cancellationToken);
        if (existingRole is not null)
            return Result<Guid>.Failure($"Ya existe un rol con el nombre '{command.Dto.Name}'.");

        var role = command.Dto.ToEntity();
        await _roleRepository.AddAsync(role, cancellationToken);

        return Result<Guid>.Success(role.Id);
    }
}