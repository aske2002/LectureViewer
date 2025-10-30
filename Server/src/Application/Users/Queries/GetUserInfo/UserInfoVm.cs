using backend.Application.Common.Models;
using backend.Contracts.Enums;

namespace backend.Application.Users.Queries.GetUserInfo;

public record UserInfoDto
{
    public required string Id { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, UserInfoDto>();
        }
    }
}
public record UserInfoVm
{
    public required UserInfoDto Info { get; init; }
    public required List<RoleDto> Roles { get; init; } = new();
    public required List<PolicyDto> Policies { get; init; } = new();
}
