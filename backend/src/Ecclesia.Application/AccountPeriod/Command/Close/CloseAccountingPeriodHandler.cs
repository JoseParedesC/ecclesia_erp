using Ecclesia.Application.AccountingPeriods.DTOs;
using Ecclesia.Application.Auth.Services;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.AccountingPeriods.Commands.CloseAccountingPeriod;

public class CloseAccountingPeriodHandler
{
    private readonly IAccountingPeriodRepository _repository;
    private readonly CloseAccountingPeriodValidator _validator;
    private readonly ICurrentUserService _currentUser;

    public CloseAccountingPeriodHandler(IAccountingPeriodRepository repository, CloseAccountingPeriodValidator validator, ICurrentUserService currentUser)
    {
        _repository  = repository;
        _validator   = validator;
        _currentUser = currentUser;
    }

    public async Task<Result<AccountingPeriodDetailDto>> HandleAsync(CloseAccountingPeriodCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AccountingPeriodDetailDto>.Failure(errors);
        }

        var entity = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (entity is null)
            return Result<AccountingPeriodDetailDto>.Failure([$"Período contable con Id {command.Id} no encontrado."]);

        if (entity.IsClosed)
            return Result<AccountingPeriodDetailDto>.Failure(["El período ya está cerrado."]);

        entity.Close(_currentUser.UserId);

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return Result<AccountingPeriodDetailDto>.Success(updated.ToDto());
    }
}