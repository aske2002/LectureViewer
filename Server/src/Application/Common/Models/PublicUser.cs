namespace backend.Application.Common.Models;

public record PublicUser
{
    public required string Id { get; init; }
    public string? UserName { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, PublicUser>();
        }
    }
}
