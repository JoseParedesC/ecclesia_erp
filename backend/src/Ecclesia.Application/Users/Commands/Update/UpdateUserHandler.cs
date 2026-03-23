using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Users.Commands.UpdateUser;

public class UpdateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly UpdateUserValidator _validator;

    public UpdateUserHandler(IUserRepository userRepository, UpdateUserValidator validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<Result<Guid>> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        // Validación
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<Guid>.Failure(errors);
        }

        // Verificar que existe
        var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
        if (user is null)
            return Result<Guid>.Failure($"Usuario con Id '{command.Id}' no encontrado.");

        // Verificar email duplicado en otro usuario
        var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingUser is not null && existingUser.Id != command.Id)
            return Result<Guid>.Failure("Ya existe un usuario con ese email.");

        // Actualizar
        user.Name = command.Name;
        user.Email = command.Email;

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}