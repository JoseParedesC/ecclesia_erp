using Ecclesia.Application.ThirdParty.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities.ThirdParty;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.ThirdParty.Queries.ListThirdParties;

public class ListThirdPartiesHandler
{
    private readonly IThirdPartyRepository _repository;
    private readonly ListThirdPartiesValidator _validator;

    public ListThirdPartiesHandler(IThirdPartyRepository repository, ListThirdPartiesValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<PagedResult<ThirdPartyListDto>>> HandleAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<PagedResult<ThirdPartyListDto>>.Failure(errors);
        }

        var result = await _repository.ListAllAsync(query, cancellationToken);
        var mapped = new PagedResult<ThirdPartyListDto>(
            result.Items.Select(x => x.ToListDto()),
            result.TotalCount,
            result.Page,
            result.PageSize
        );

        return Result<PagedResult<ThirdPartyListDto>>.Success(mapped);
    }
}