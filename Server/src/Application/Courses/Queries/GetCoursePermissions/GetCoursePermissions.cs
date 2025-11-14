using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Courses.Queries.GetCourseById;

[Authorize]
public record GetCoursePermissionsQuery(CourseId Id) : IRequest<CoursePermissionsDto>;

public class GetCoursePermissionsQueryHandler : IRequestHandler<GetCoursePermissionsQuery, CoursePermissionsDto>
{
    private readonly ICourseService _service;

    public GetCoursePermissionsQueryHandler(ICourseService service)
    {
        _service = service;
    }

    public async Task<CoursePermissionsDto> Handle(GetCoursePermissionsQuery request, CancellationToken cancellationToken)
    {
        var response = await _service.GetUserCoursePermissionsAsync(request.Id);
        return new CoursePermissionsDto
        {
            Permissions = response
        };
    }
}
