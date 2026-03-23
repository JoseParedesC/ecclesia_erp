using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.Users;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Users.Queries;

public class GetUserByIdHandler
{
    private readonly IUserRepository _userRepository;
    private readonly GetUserByIdValidator _validator;

    public GetUserByIdHandler(IUserRepository userRepository, GetUserByIdValidator validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<Result<UserEntity>> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        // Validación
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<UserEntity>.Failure(errors);
        }

        // Consulta
        var user = await _userRepository.GetByIdNoTrackAsync(query.Id, cancellationToken);
        if (user is null)
            return Result<UserEntity>.Failure($"Usuario con Id '{query.Id}' no encontrado.");

        return Result<UserEntity>.Success(user);
    }
}