using System.Security.Claims;

namespace backend.Application.Common.Interfaces;

public interface IUserAccessor
{
    ClaimsPrincipal? Principal { get; }
    ClaimsPrincipal GetPrincipal();

    Task<bool> TryGetCurrentUserAsync(out ApplicationUser? user);
    Task<ApplicationUser> GetCurrentUserAsync();
}