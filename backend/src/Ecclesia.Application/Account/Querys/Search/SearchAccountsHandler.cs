using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Accounts.Queries.SearchAccounts;

public class SearchAccountsHandler
{
    private readonly IAccountRepository _repository;
    private readonly SearchAccountValidator _validator;

    public SearchAccountsHandler(IAccountRepository repository, SearchAccountValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<PagedResult<AccountSearchDto>>> HandleAsync(SearchAccountQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<PagedResult<AccountSearchDto>>.Failure(errors);
        }

        var result = await _repository.SearchAsync(query.Search, query.Page, query.PageSize, cancellationToken);

        var mapped = new PagedResult<AccountSearchDto>(
            result.Items.Select(x => x.ToSearchDto()),
            result.TotalCount,
            result.Page,
            result.PageSize
        );

        return Result<PagedResult<AccountSearchDto>>.Success(mapped);
    }
}