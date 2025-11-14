using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Common;
using backend.Domain.Constants;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Events;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace backend.Application.Lectures.Commands.CreateCourse;

public record UploadLectureMediaCommand : IRequest<LectureContent>
{
    public required CourseId CourseId { get; init; }
    public required LectureId LectureId { get; init; }
    public required IFormFile File { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public bool IsMainContent { get; init; } = false;
}

public class UploadLectureCommandHandler : IRequestHandler<UploadLectureMediaCommand, LectureContent>
{
    private readonly ICourseService _courseService;
    private readonly IUserAccessor _userAccessor;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMediator _mediator;

    public UploadLectureCommandHandler(ICourseService courseService, IUserAccessor userAccessor, IAuthorizationService authorizationService, IMediator mediator)
    {
        _courseService = courseService;
        _userAccessor = userAccessor;
        _authorizationService = authorizationService;
        _mediator = mediator;
    }

    public async Task<LectureContent> Handle(UploadLectureMediaCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseService.GetCourseDetailsAsync(request.CourseId);

        var authorizationResult = await _authorizationService.AuthorizeAsync(_userAccessor.GetPrincipal(), course, CoursePermissions.FromCoursePermission(CoursePermissions.CoursePermissionType.UploadMedia));
        if (!authorizationResult.Succeeded)
        {
            throw new UnauthorizedAccessException("You do not have permission to upload lecture media for this course.");
        }

        var lectureContent = await _courseService.UploadLectureMaterialAsync(
            courseId: request.CourseId,
            lectureId: request.LectureId,
            file: request.File,
            contentType: MimeTypeMappings.GetLectureContentType(request.File.ContentType),
            name: request.Name,
            description: request.Description,
            isMainContent: request.IsMainContent,
            cancellationToken: cancellationToken
        );

        await _mediator.Publish(new MediaUploadedEvent(
            CourseId: request.CourseId,
            LectureId: request.LectureId,
            LectureContentId: lectureContent.Id
        ), cancellationToken);

        return lectureContent;
    }
}
