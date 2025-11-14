using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class CourseEnrollment : BaseAuditableEntity<CourseEnrollmentId>
{
    public CourseInviteLinkId? InviteLinkId { get; init; }
    public CourseInviteLink? InviteLink { get; init; }
    public required CourseId CourseId { get; init; } = CourseId.Default();
    public required Course Course { get; init; } = null!;
    public required ApplicationUser User { get; init; }
    public IList<FlashcardAnswer> FlashcardAnswers { get; init; } = new List<FlashcardAnswer>();
}