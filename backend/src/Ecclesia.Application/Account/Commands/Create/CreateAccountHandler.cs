using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.Account;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Accounts.Commands.CreateAccount;

public class CreateAccountHandler
{
    private readonly IAccountRepository _repository;
    private readonly CreateAccountValidator _validator;

    public CreateAccountHandler(IAccountRepository repository, CreateAccountValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<AccountDetailDto>> HandleAsync(CreateAccountCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AccountDetailDto>.Failure(errors);
        }

        if (await _repository.ExistsByCodeAsync(command.Code!, cancellationToken: cancellationToken))
            return Result<AccountDetailDto>.Failure([$"Ya existe una cuenta con el código {command.Code}."]);

        var entity = new AccountEntity(
            code:            command.Code,
            name:            command.Name,
            type:            command.Type,
            parentAccountId: command.ParentAccountId
        );

        var created = await _repository.CreateAsync(entity, cancellationToken);
        return Result<AccountDetailDto>.Success(created.ToDto());
    }
}