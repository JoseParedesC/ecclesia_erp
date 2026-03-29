
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.AccountingPeriod;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.AccountingPeriod.Commands.CreateAccountingPeriod;

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

        var oldPeriod = await _repository.GetPeriodAsync(command.Period, cancellationToken);
        if(oldPeriod == null)
            throw new Exception("Current period not found. Must be Opened to create a new period");
        if (oldPeriod != null && oldPeriod.Status == StatusDocument.CLOSED)
            throw new Exception("Current period is closed. Must be Opened to create a new period");


        AccountingPeriodEntity periodToCreate = new AccountingPeriodEntity(
            year: oldPeriod.Year,
            month: oldPeriod.Month
        );
        var newPeriod = await _repository.GetPeriodAsync(periodToCreate.GetPeriodDate(), cancellationToken);
        if (newPeriod != null)
            throw new Exception($"Accounting period alredy exist and is {newPeriod.Status}");

            
        // var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        // if (!validationResult.IsValid)
        // {
        //     var errors = validationResult.Errors.Select(e => e.ErrorMessage);
        //     return Result<AccountingPeriodDetailDto>.Failure(errors);
        // }



        var entity = new AccountingPeriodEntity(
            year: command.Period.Year,
            month: command.Period.Month
        );

        var created = await _repository.CreateOpenPeriodAsync(entity, cancellationToken);

        if(created == null)
        {
            throw new Exception("An error was ocurred creating the new period");
        }

        oldPeriod.Close();
        
        _repository.

        return Result<AccountingPeriodDetailDto>.Success(created.ToDetailDto());
    }
}