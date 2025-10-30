using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class UserNotEnrolledException  : Exception
{
    public UserNotEnrolledException(string userId, CourseId courseId)
        : base($"User with ID {userId} is not enrolled in course {courseId}.") { }
}