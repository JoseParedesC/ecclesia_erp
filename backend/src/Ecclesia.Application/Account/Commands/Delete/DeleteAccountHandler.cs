using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Accounts.Commands.DeleteAccount;

public class DeleteAccountHandler
{
    private readonly IAccountRepository _repository;
    private readonly DeleteAccountValidator _validator;

    public DeleteAccountHandler(IAccountRepository repository, DeleteAccountValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<AccountDetailDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var command          = new DeleteAccountCommand(id);
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AccountDetailDto>.Failure(errors);
        }

        var entity = await _repository.DeleteAsync(id, cancellationToken);
        return Result<AccountDetailDto>.Success(entity.ToDto());
    }
}