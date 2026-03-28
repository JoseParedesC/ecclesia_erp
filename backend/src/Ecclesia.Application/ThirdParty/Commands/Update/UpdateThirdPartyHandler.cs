using Ecclesia.Application.ThirdParty.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.ThirdParty;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.ThirdParty.Commands.UpdateThirdParty;

public class UpdateThirdPartyHandler
{
    private readonly IThirdPartyRepository _repository;
    private readonly UpdateThirdPartyValidator _validator;

    public UpdateThirdPartyHandler(IThirdPartyRepository repository, UpdateThirdPartyValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<ThirdPartyDetailDto>> HandleAsync(UpdateThirdPartyCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<ThirdPartyDetailDto>.Failure(errors);
        }

        var entity = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (entity is null)
            return Result<ThirdPartyDetailDto>.Failure([$"ThirdParty con Id {command.Id} no encontrado."]);

        entity.Update(
            firstName:    command.FirstName,
            lastName:     command.LastName,
            businessName: command.BusinessName,
            tradeName:    command.TradeName,
            email:        command.Email,
            phone:        command.Phone,
            address:      command.Address,
            city:         command.City,
            country:      command.Country
        );

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return Result<ThirdPartyDetailDto>.Success(updated.ToDto());
    }
}