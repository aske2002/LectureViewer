using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class CourseInviteLinkNotFoundException : NotFoundExceptionBase<CourseInviteLinkId, CourseInviteLinkNotFoundException>
{
    public override string EntityName => nameof(CourseInviteLink);
    public CourseInviteLinkNotFoundException(CourseInviteLinkId inviteLinkId)
        : base(inviteLinkId) { }
}