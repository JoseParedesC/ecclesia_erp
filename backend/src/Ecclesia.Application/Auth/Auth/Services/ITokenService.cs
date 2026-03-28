using Ecclesia.Domain.Entities.Users;

namespace Ecclesia.Application.Auth.Services;

public interface ITokenService
{
    string GenerateToken(UserEntity user, IEnumerable<string> permissions);
    DateTime GetExpiration();
}