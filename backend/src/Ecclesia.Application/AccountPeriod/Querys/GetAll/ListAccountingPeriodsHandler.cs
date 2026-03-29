using Ecclesia.Application.AccountingPeriods.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.AccountingPeriods.Queries.ListAccountingPeriods;

public class ListAccountingPeriodsHandler
{
    private readonly IAccountingPeriodRepository _repository;
    private readonly ListAccountingPeriodsValidator _validator;

    public ListAccountingPeriodsHandler(IAccountingPeriodRepository repository, ListAccountingPeriodsValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<PagedResult<AccountingPeriodListDto>>> HandleAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<PagedResult<AccountingPeriodListDto>>.Failure(errors);
        }

        var result = await _repository.ListAllAsync(query, cancellationToken);

        var mapped = new PagedResult<AccountingPeriodListDto>(
            result.Items.Select(x => x.ToListDto()),
            result.TotalCount,
            result.Page,
            result.PageSize
        );

        return Result<PagedResult<AccountingPeriodListDto>>.Success(mapped);
    }
}