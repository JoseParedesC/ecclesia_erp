using Ecclesia.Application.AccountingPeriods.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.AccountingPeriod;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.AccountingPeriods.Commands.CreateAccountingPeriod;

public class CreateAccountingPeriodHandler
{
    private readonly IAccountingPeriodRepository _repository;
    private readonly CreateAccountingPeriodValidator _validator;

    public CreateAccountingPeriodHandler(IAccountingPeriodRepository repository, CreateAccountingPeriodValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<AccountingPeriodDetailDto>> HandleAsync(CreateAccountingPeriodCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AccountingPeriodDetailDto>.Failure(errors);
        }

        // Verificar que no exista el período
        if (await _repository.ExistsByYearMonthAsync(command.Year, command.Month, cancellationToken))
            return Result<AccountingPeriodDetailDto>.Failure([$"Ya existe un período para {command.Month}/{command.Year}."]);

        var entity  = new AccountingPeriodEntity(command.Year, command.Month);
        var created = await _repository.CreateAsync(entity, cancellationToken);
        return Result<AccountingPeriodDetailDto>.Success(created.ToDto());
    }
}