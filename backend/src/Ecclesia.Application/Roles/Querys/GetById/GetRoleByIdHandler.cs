using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.Roles;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Roles.Queries;

public class GetRoleByIdHandler
{
    private readonly IRoleRepository _RoleRepository;
    private readonly GetRoleByIdValidator _validator;

    public GetRoleByIdHandler(IRoleRepository RoleRepository, GetRoleByIdValidator validator)
    {
        _RoleRepository = RoleRepository;
        _validator = validator;
    }

    public async Task<Result<RoleEntity>> HandleAsync(GetRoleByIdQuery query, CancellationToken cancellationToken = default)
    {
        // Validación
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<RoleEntity>.Failure(errors);
        }

        // Consulta
        var Role = await _RoleRepository.GetByIdNoTrackAsync(query.Id, cancellationToken);
        if (Role is null)
            return Result<RoleEntity>.Failure($"Usuario con Id '{query.Id}' no encontrado.");

        return Result<RoleEntity>.Success(Role);
    }
}