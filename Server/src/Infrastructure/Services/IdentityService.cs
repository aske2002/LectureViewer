using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Models;
using backend.Domain.Constants;
using backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace backend.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly EmailAddressAttribute _emailAddressAttribute = new();


    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IUserStore<ApplicationUser> userStore,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _userStore = userStore;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<(Result Result, ApplicationUser? User)> CreateUserAsync(string email, string password, string firstName, string lastName)
    {

        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException($"{nameof(IdentityService)} requires a user store with email support.");
        }

        var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;

        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            return (IdentityResult.Failed(_userManager.ErrorDescriber.InvalidEmail(email)).ToApplicationResult(), null);
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };

        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return (result.ToApplicationResult(), null);
        }
        return (result.ToApplicationResult(), user);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    public Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal principal)
    {
        return _userManager.GetUserAsync(principal);
    }

    public Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return _userManager.FindByIdAsync(userId);
    }

    public async Task<ICollection<string>> GetUserPoliciesAsync(ApplicationUser user)
    {

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var allPolicies = await Task.WhenAll(typeof(Policies).GetFields()
            .Where(f => f.IsStatic && f.IsLiteral)
            .Select(f => f.GetValue(null)?.ToString() ?? "")
            .ToList()
            .Select(async p =>
            {
                var result = await _authorizationService.AuthorizeAsync(principal, p);
                return (Policy: p, IsAuthorized: result.Succeeded);
            }))
            .ContinueWith(t => t.Result.Where(r => r.IsAuthorized).Select(r => r.Policy).ToList());

        return allPolicies;
    }

    public async Task<ICollection<string>> GetUserRolesAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles;
    }
}
