using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Users.Commands.DeleteUser;

public class DeleteUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly DeleteUserValidator _validator;

    public DeleteUserHandler(IUserRepository userRepository, DeleteUserValidator validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<Result<Guid>> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
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

        await _userRepository.DeleteAsync(user, cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}