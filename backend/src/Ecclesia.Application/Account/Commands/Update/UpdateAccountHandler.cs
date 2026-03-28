using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Accounts.Commands.UpdateAccount;

public class UpdateAccountHandler
{
    private readonly IAccountRepository _repository;
    private readonly UpdateAccountValidator _validator;

    public UpdateAccountHandler(IAccountRepository repository, UpdateAccountValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<AccountDetailDto>> HandleAsync(UpdateAccountCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AccountDetailDto>.Failure(errors);
        }

        var entity = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (entity is null)
            return Result<AccountDetailDto>.Failure([$"Account con Id {command.Id} no encontrada."]);

        if (await _repository.ExistsByCodeAsync(command.Code!, excludeId: command.Id, cancellationToken: cancellationToken))
            return Result<AccountDetailDto>.Failure([$"Ya existe una cuenta con el código {command.Code}."]);
            

        entity.Update(
            code:            command.Code,
            name:            command.Name,
            type:            command.Type,
            parentAccountId: command.ParentAccountId
        );

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return Result<AccountDetailDto>.Success(updated.ToDto());
    }
}