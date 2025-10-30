using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class UserAlreadyEnrolledException : Exception
{
    public UserAlreadyEnrolledException(ApplicationUser user, CourseId courseId)
        : base($"User with ID {user.Id} is already enrolled in course {courseId}.") { }
}