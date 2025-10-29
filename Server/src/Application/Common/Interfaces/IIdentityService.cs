using backend.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, ApplicationUser? User)> CreateUserAsync(string userName, string password, string firstName, string lastName);

    Task<Result> DeleteUserAsync(string userId);
}
