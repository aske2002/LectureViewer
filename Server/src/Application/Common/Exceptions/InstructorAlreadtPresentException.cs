using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class InstructorAlreadyPresentException : Exception
{
    public InstructorAlreadyPresentException(ApplicationUser user, CourseId courseId)
        : base($"User with ID {user.Id} is already an instructor in course {courseId}.") { }
}