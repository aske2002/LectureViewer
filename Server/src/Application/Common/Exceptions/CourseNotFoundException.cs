using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class CourseNotFoundException : NotFoundExceptionBase<CourseId, CourseNotFoundException>
{
    public override string EntityName => nameof(Course);
    public CourseNotFoundException(CourseId courseId)
        : base(courseId) { }
}