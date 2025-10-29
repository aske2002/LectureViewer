using backend.Application.Common.Interfaces;
using backend.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<(Result Result, ApplicationUser? User)>
{
    /// <summary>
    /// The user's email address which acts as a user name.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public required string Password { get; init; }
    /// <summary>
    /// The user's first name.
    /// </summary>
    public required string FirstName { get; init; }
    /// <summary>
    /// The user's last name.
    /// </summary>
    public required string LastName { get; init; }
}
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, (Result Result, ApplicationUser? User)>
{
    private readonly IIdentityService _identityService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateUserCommandHandler(IIdentityService identityService, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<(Result Result, ApplicationUser? User)> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        return await _identityService.CreateUserAsync(request.Email, request.Password, request.FirstName, request.LastName);
    }
}
