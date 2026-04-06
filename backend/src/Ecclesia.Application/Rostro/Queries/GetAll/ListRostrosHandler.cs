using Ecclesia.Application.Rostros.Queries.ListRostros;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Common.PagedQuery;
using Ecclesia.Domain.Entities;
using Ecclesia.Domain.Repositories;

public class ListRostrosHandler
{
    private readonly IRostroRepository _repository;
    private readonly ListRostrosValidator _validator;

    public ListRostrosHandler(IRostroRepository repository, ListRostrosValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<PagedResult<RostroEntity>>> HandleAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<PagedResult<RostroEntity>>.Failure(errors);
        }

        var result = await _repository.ListAsync(query, cancellationToken);

        return Result<PagedResult<RostroEntity>>.Success(result);
    }
}