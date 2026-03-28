using Ecclesia.Application.ThirdParty.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.ThirdParty;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.ThirdParty.Commands.DeleteThirdParty;

public class DeleteThirdPartyHandler
{
    private readonly IThirdPartyRepository _repository;
    private readonly DeleteThirdPartyValidator _validator;

    public DeleteThirdPartyHandler(IThirdPartyRepository repository, DeleteThirdPartyValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<ThirdPartyDetailDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var command          = new DeleteThirdPartyCommand(id);
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<ThirdPartyDetailDto>.Failure(errors);
        }

        var entity = await _repository.DeleteAsync(id, cancellationToken);
        return Result<ThirdPartyDetailDto>.Success(entity.ToDto());
    }
}