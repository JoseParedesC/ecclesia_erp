namespace Ecclesia.Application.Auth.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
    string UserName { get; }
    IEnumerable<string> Permissions { get; }
    bool IsAuthenticated { get; }
}