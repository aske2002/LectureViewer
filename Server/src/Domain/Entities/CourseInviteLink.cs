using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class CourseInviteLink : BaseAuditableEntity<CourseInviteLinkId>
{
    public DateTimeOffset ExpirationDate { get; init; }
    public string? Title { get; init; }
    public required CourseId CourseId { get; init; } = CourseId.Default();
    public required Course Course { get; init; } = null!;
}