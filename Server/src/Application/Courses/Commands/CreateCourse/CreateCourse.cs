using backend.Application.Common.Exceptions;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Common;
using backend.Domain.Constants;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Courses.Commands.CreateCourse;

public record CreateCourseCommand : IRequest<CourseId>
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public required string InternalIdentifier { get; init; }
    public Season SemesterSeason { get; init; }
    public int SemesterYear { get; init; }
}

[Authorize(Policy = Policies.CanCreateCourses)]
public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseId>
{
    private readonly ICourseService _courseService;
    private readonly IUserAccessor _userAccessor;
    public CreateCourseCommandHandler(ICourseService courseService, IUserAccessor userAccessor)
    {
        _courseService = courseService;
        _userAccessor = userAccessor;
    }
    public async Task<CourseId> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var user = await _userAccessor.GetCurrentUserAsync();
        if (user == null)
        {
            throw new ForbiddenAccessException();
        }

        var course = await _courseService.CreateCourseAsync(
            owner: user,
            internalIdentifier: request.InternalIdentifier,
            name: request.Name,
            description: request.Description,
            semesterSeason: request.SemesterSeason,
            semesterYear: request.SemesterYear);

        return course.Id;
    }
}
