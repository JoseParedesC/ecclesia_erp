using Ecclesia.Application.Users.DTOs;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Entities.Users;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Users.Commands.CreateUser;

public class CreateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly CreateUserValidator _validator;

    public CreateUserHandler(IUserRepository userRepository, CreateUserValidator validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<Result<Guid>> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
{
    // Validación
    var validationResult = await _validator.ValidateAsync(command, cancellationToken);
    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors.Select(e => e.ErrorMessage);
        return Result<Guid>.Failure(errors);
    }

    // Verificar email duplicado
    var existingUser = await _userRepository.GetByEmailAsync(command.userDto.Email, cancellationToken);
    if (existingUser is not null)
        return Result<Guid>.Failure("Ya existe un usuario con ese email.");

    // Crear entidad usando el mapper
    var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.userDto.Password);
    var user = command.userDto.ToEntity(passwordHash);

    await _userRepository.AddAsync(user, cancellationToken);

    return Result<Guid>.Success(user.Id);
}
}