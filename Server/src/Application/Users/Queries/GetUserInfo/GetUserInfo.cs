using System.Security.Claims;
using backend.Application.Common.Exceptions;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Mappings;
using backend.Application.Common.Security;
using backend.Domain.Constants;
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
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetUserInfoQueryHandler(IMapper mapper, IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<UserInfoVm?> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        if (await _identityService.GetUserAsync(request.User) is not { } user)
        {
            throw new UserNotFoundException(request.User.Identity?.Name ?? "");
        }
        var roles = await _identityService.GetUserRolesAsync(user);
        var policies = await _identityService.GetUserPoliciesAsync(user);

        return new UserInfoVm
        {
            Info = _mapper.Map<UserInfoDto>(user),
            Roles = RolePolicyMapper.ToRoleDtos(roles),
            Policies = RolePolicyMapper.ToPolicyDtos(policies)
        };
    }
}
