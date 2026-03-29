using Ecclesia.Application.AccountingPeriods.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.AccountingPeriods.Queries.GetAccountingPeriodById;

public class GetAccountingPeriodByIdHandler
{
    private readonly IAccountingPeriodRepository _repository;
    private readonly GetAccountingPeriodByIdValidator _validator;

    public GetAccountingPeriodByIdHandler(IAccountingPeriodRepository repository, GetAccountingPeriodByIdValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<AccountingPeriodDetailDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query            = new GetAccountingPeriodByIdQuery(id);
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AccountingPeriodDetailDto>.Failure(errors);
        }

        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result<AccountingPeriodDetailDto>.Failure([$"Período contable con Id {id} no encontrado."]);

        return Result<AccountingPeriodDetailDto>.Success(entity.ToDto());
    }
}