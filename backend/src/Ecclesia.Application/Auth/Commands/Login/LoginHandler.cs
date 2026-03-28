using Ecclesia.Application.Auth.DTOs;
using Ecclesia.Application.Auth.Services;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Auth.Commands.Login;

public class LoginHandler
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;
    private readonly LoginValidator _validator;

    public LoginHandler(IAuthRepository authRepository, ITokenService tokenService, LoginValidator validator)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
        _validator = validator;
    }

    public async Task<Result<AuthResponseDto>> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<AuthResponseDto>.Failure(errors);
        }

        // Verificar usuario
        var user = await _authRepository.GetByEmailAsync(command.Dto.Email, cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(command.Dto.Password, user.PasswordHash))
            return Result<AuthResponseDto>.Failure("Credenciales inválidas.");

        // Obtener permisos
        var permissions = await _authRepository.GetUserPermissionsAsync(user.Id, cancellationToken);

        // Generar token
        var token = _tokenService.GenerateToken(user, permissions);

        return Result<AuthResponseDto>.Success(new AuthResponseDto(
            user.Id,
            user.Name,
            user.UserName,
            user.Email,
            token,
            _tokenService.GetExpiration()
        ));
    }
}