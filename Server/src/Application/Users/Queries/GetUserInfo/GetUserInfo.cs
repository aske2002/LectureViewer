using System.Security.Claims;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Mappings;
using backend.Application.Common.Models;
using backend.Application.Common.Security;
using backend.Application.Countries.Queries.GetCountries;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace backend.Application.Users.Queries.GetUserInfo;

[Authorize]
public record GetUserInfoQuery : IRequest<UserInfoVm?>
{
    public required ClaimsPrincipal User { get; init; }
}

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoVm?>
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    public GetUserInfoQueryHandler(IMapper mapper, UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<UserInfoVm?> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        if (await _userManager.GetUserAsync(request.User) is not { } user)
        {
            throw new NotFoundException(request.User.Identity?.Name ?? "", "User");
        }
        return new UserInfoVm
        {
            Info = _mapper.Map<UserInfoDto>(user),
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };
    }
}
