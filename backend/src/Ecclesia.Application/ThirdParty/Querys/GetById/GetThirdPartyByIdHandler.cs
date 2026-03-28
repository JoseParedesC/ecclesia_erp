using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.ThirdParty;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.ThirdParty.Queries.GetThirdPartyById;

public class GetThirdPartyByIdHandler
{
    private readonly IThirdPartyRepository _repository;
    private readonly GetThirdPartyByIdValidator _validator;

    public GetThirdPartyByIdHandler(IThirdPartyRepository repository, GetThirdPartyByIdValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<ThirdPartyEntity>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query            = new GetThirdPartyByIdQuery(id);
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<ThirdPartyEntity>.Failure(errors);
        }

        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result<ThirdPartyEntity>.Failure([$"ThirdParty con Id {id} no encontrado."]);

        return Result<ThirdPartyEntity>.Success(entity);
    }
}