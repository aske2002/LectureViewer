using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Common;
using backend.Domain.Constants;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Courses.Commands.CreateLecture;

public record CreateLectureCommand : IRequest<LectureId>
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required CourseId CourseId { get; init; }
}

[Authorize(Policy = Policies.CanCreateCourses)]
public class CreateLectureCommandHandler : IRequestHandler<CreateLectureCommand, LectureId>
{
    private readonly ICourseService _courseService;
    private readonly IIdentityService _identityService;
    public CreateLectureCommandHandler(ICourseService courseService, IIdentityService identityService)
    {
        _courseService = courseService;
        _identityService = identityService;
    }
    public async Task<LectureId> Handle(CreateLectureCommand request, CancellationToken cancellationToken)
    {
        var lecture = await _courseService.CreateLectureAsync(
            courseId: request.CourseId,
            startDate: request.StartDate,
            endDate: request.EndDate,
            title: request.Title,
            description: request.Description);


        return lecture.Id;
    }
}
