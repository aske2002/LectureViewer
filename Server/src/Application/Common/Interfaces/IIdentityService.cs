using System.Security.Claims;
using backend.Application.Common.Models;

namespace backend.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, ApplicationUser? User)> CreateUserAsync(string userName, string password, string firstName, string lastName);

    Task<Result> DeleteUserAsync(string userId);
    Task<ApplicationUser?> GetUserAsync(System.Security.Claims.ClaimsPrincipal principal);
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<ICollection<string>> GetUserPoliciesAsync(ApplicationUser user);
    Task<ICollection<string>> GetUserRolesAsync(ApplicationUser user);
}
