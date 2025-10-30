using System.Security.Claims;
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

public record EnrollCourseCommand : IRequest<CourseEnrollmentId>
{
    public required string Token { get; init; }
    public required CourseId CourseId { get; init; }
    public required ClaimsPrincipal User { get; init; }
}

[Authorize(Policy = Policies.CanCreateCourses)]
public class EnrollCourseCommandHandler : IRequestHandler<EnrollCourseCommand, CourseEnrollmentId>
{
    private readonly ICourseService _courseService;
    private readonly IIdentityService _identityService;

    public EnrollCourseCommandHandler(ICourseService courseService, IIdentityService identityService)
    {
        _courseService = courseService;
        _identityService = identityService;
    }
    public async Task<CourseEnrollmentId> Handle(EnrollCourseCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserAsync(request.User);

        if (user is null)
        {
            throw new UnauthorizedAccessException("User must be logged in to enroll in a course.");
        }

        var enrollment = await _courseService.EnrollUserInCourseAsync(user, courseId: request.CourseId, inviteToken: request.Token);
        return enrollment.Id;
    }
}
