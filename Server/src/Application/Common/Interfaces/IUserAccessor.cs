using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace backend.Application.Common.Interfaces;

public interface IUserAccessor
{
    ClaimsPrincipal Principal { get; }
    Task<ApplicationUser> GetCurrentUserAsync();
}
public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IIdentityService _identityService;

    public UserAccessor(IHttpContextAccessor accessor, IIdentityService identityService)
    {
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
    }

    public ClaimsPrincipal Principal => _accessor.HttpContext.User;

    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        return await _identityService.GetUserAsync(Principal) 
               ?? throw new InvalidOperationException("Current user not found");
    }
}