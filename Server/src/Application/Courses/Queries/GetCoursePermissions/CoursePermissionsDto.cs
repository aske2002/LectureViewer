using static backend.Domain.Constants.CoursePermissions;

namespace backend.Application.Courses.Queries.GetCourseById;

public record CoursePermissionsDto
{
    public ICollection<CoursePermissionType> Permissions { get; init; } = new List<CoursePermissionType>();
}

