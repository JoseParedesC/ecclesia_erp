using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.Roles;
using Ecclesia.Domain.Repositories;
using Ecclesia.Domain.Common.PagedQuery;

namespace Ecclesia.Application.Roles.Queries.GetAllRoles;

public class GetAllRolesHandler
{
    private readonly IRoleRepository _roleRepository;
    private readonly GetAllRolesValidator _validator;

    public GetAllRolesHandler(IRoleRepository roleRepository, GetAllRolesValidator validator)
    {
        _roleRepository = roleRepository;
        _validator = validator;
    }

    public async Task<Result<PagedResult<RoleEntity>>> HandleAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<PagedResult<RoleEntity>>.Failure(errors);
        }

        var result = await _roleRepository.ListAllAsync(query, cancellationToken);

        return Result<PagedResult<RoleEntity>>.Success(result);
    }
}