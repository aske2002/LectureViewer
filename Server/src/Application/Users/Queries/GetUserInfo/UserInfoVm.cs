using backend.Application.Common.Models;

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
    public required List<string> Roles { get; init; } = new();
}
