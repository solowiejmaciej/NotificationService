#region

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

#endregion

namespace AuthService.Application.ApplicationUserContext;

public interface IUserContext
{
    CurrentUser GetCurrentUser();
}

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUser GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext!.User;

        var userName = user.Identity.Name!;
        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        //var userRole = user.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;

        return new CurrentUser(userId, userName);
    }
}