using Ecclesia.Application.Accounts.Queries.SearchAccounts;
using Ecclesia.Application.Roles.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Roles.Queries.SearchRoles;

public class SearchRolesHandler
{
    private readonly IRoleRepository _repository;
    private readonly SearchRoleValidator _validator;

    public SearchRolesHandler(IRoleRepository repository, SearchRoleValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<PagedResult<SearchRoleDto>>> HandleAsync(SearchRoleQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<PagedResult<SearchRoleDto>>.Failure(errors);
        }

        var result = await _repository.SearchAsync(query.Search, query.Page, query.PageSize, cancellationToken);

        var mapped = new PagedResult<SearchRoleDto>(
            result.Items.Select(x => x.ToSearchDto()),
            result.TotalCount,
            result.Page,
            result.PageSize
        );

        return Result<PagedResult<SearchRoleDto>>.Success(mapped);
    }
}