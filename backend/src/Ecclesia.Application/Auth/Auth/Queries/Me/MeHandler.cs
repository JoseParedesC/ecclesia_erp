using Ecclesia.Application.Auth.DTOs;
using Ecclesia.Application.Auth.Services;
using Ecclesia.Domain.Common;
using Ecclesia.Domain.Repositories;

namespace Ecclesia.Application.Auth.Queries.Me;

public class MeHandler
{
    private readonly IAuthRepository _authRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;

    public MeHandler(IAuthRepository authRepository, IUserRepository userRepository, ICurrentUserService currentUser)
    {
        _authRepository = authRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<MeDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(_currentUser.UserId, cancellationToken);
        if (user is null)
            return Result<MeDto>.Failure("Usuario no encontrado.");

        var roles       = await _authRepository.GetUserRolesAsync(user.Id, cancellationToken);
        var permissions = await _authRepository.GetUserPermissionsAsync(user.Id, cancellationToken);

        return Result<MeDto>.Success(new MeDto(
            user.Id,
            user.Name,
            user.UserName,
            user.Email,
            roles,
            permissions
        ));
    }
}