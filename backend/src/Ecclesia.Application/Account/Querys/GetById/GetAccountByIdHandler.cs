using Ecclesia.Application.Accounts.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Accounts.Queries.GetAccountById;

public class GetAccountByIdHandler
{
    private readonly IAccountRepository _repository;
    private readonly GetAccountByIdValidator _validator;

    public GetAccountByIdHandler(IAccountRepository repository, GetAccountByIdValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<AccountDetailDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query            = new GetAccountByIdQuery(id);
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AccountDetailDto>.Failure(errors);
        }

        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result<AccountDetailDto>.Failure([$"Account con Id {id} no encontrada."]);

        return Result<AccountDetailDto>.Success(entity.ToDto());
    }
}