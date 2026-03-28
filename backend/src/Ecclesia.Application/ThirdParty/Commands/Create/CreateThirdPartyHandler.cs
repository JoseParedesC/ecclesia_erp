using Ecclesia.Application.ThirdParty.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.ThirdParty;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.ThirdParty.Commands.CreateThirdParty;

public class CreateThirdPartyHandler
{
    private readonly IThirdPartyRepository _repository;
    private readonly CreateThirdPartyValidator _validator;

    public CreateThirdPartyHandler(IThirdPartyRepository repository, CreateThirdPartyValidator validator)
    {
        _repository = repository;
        _validator  = validator;
    }

    public async Task<Result<ThirdPartyDetailDto>> HandleAsync(CreateThirdPartyCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<ThirdPartyDetailDto>.Failure(errors);
        }

        var entity = new ThirdPartyEntity(
            identificationNumber: command.IdentificationNumber,
            typeIden:             command.TypeIden,
            personType:           command.PersonType,
            firstName:            command.FirstName,
            lastName:             command.LastName,
            birthDate:            command.BirthDate,
            businessName:         command.BusinessName,
            tradeName:            command.TradeName,
            email:                command.Email,
            phone:                command.Phone,
            address:              command.Address,
            city:                 command.City,
            country:              command.Country
        );

        var created = await _repository.CreateAsync(entity, cancellationToken);
        return Result<ThirdPartyDetailDto>.Success(created.ToDto());
    }
}