using System.Security.Claims;
using backend.Application.Common.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace backend.Application.Common.Interfaces;

public class UserAccessor : IUserAccessor
{

    private readonly IUser _user;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserAccessor(IUser user, UserManager<ApplicationUser> userManager)
    {
        _user = user;
        _userManager = userManager;
    }
    public ClaimsPrincipal? Principal => _user.Principal;

    public ClaimsPrincipal GetPrincipal()
    {
        if (_user.Principal == null)
        {
            throw new ForbiddenAccessException();
        }
        return _user.Principal;
    }

    public Task<bool> TryGetCurrentUserAsync(out ApplicationUser? user)
    {
        var principal = _user.Principal;
        if (principal == null)
        {
            user = null;
            return Task.FromResult(false);
        }

        user = _userManager.GetUserAsync(principal).Result;
        if (user == null)
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(true);
    }
    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        var principal = _user.Principal;
        if (principal == null)
        {
            throw new ForbiddenAccessException();
        }

        var user = await _userManager.GetUserAsync(principal);
        if (user == null)
        {
            throw new ForbiddenAccessException();
        }
        return user;
    }
}