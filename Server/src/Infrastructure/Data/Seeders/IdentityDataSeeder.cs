
using backend.Application.Common.Interfaces;
using backend.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace backend.Infrastructure.Data.Seeders;

public class IdentityDataSeeder : IDataSeeder
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityDataSeeder(IApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost.dk", Email = "administrator@localhost.dk", FirstName = "Administrator", LastName = "Administrator" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ShouldSeedAsync()
    {
        return await _context.Database.CanConnectAsync() && !_context.Users.Any();
    }
}
