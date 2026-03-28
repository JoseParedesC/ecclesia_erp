using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Accounts.Queries.ListAccounts;

public class ListAccountsHandler
{
    private readonly IAccountRepository _repository;
    private readonly ListAccountsValidator _validator;

    public ListAccountsHandler(IAccountRepository repository, ListAccountsValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<PagedResult<AccountListDto>>> HandleAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<PagedResult<AccountListDto>>.Failure(errors);
        }

        var result = await _repository.ListAllAsync(query, cancellationToken);

        var mapped = new PagedResult<AccountListDto>(
            result.Items.Select(x => x.ToListDto()),
            result.TotalCount,
            result.Page,
            result.PageSize
        );

        return Result<PagedResult<AccountListDto>>.Success(mapped);
    }
}