using System.Security.Claims;
using Ecclesia.Application.Auth.Services;
using Microsoft.AspNetCore.Http;

namespace Ecclesia.Infrastructure.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        Guid.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User?.FindFirstValue("sub"), out var id) ? id : Guid.Empty;

    public string Email =>
        User?.FindFirstValue(ClaimTypes.Email)
        ?? User?.FindFirstValue("email")
        ?? string.Empty;

    public string UserName =>
        User?.FindFirstValue("username") ?? string.Empty;

    public IEnumerable<string> Permissions =>
        User?.FindAll("permission").Select(c => c.Value) ?? Enumerable.Empty<string>();

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;
}