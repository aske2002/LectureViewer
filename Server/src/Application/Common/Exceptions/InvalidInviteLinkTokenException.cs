using backend.Domain.Entities;

namespace backend.Application.Common.Exceptions;
public class InvalidInviteLinkTokenException : Exception
{
    public InvalidInviteLinkTokenException(Course course)
        : base($"The invite link token for course with ID {course.Id} is invalid or has expired.") { }
}