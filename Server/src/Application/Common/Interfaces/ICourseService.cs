using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Http;
using static backend.Domain.Constants.CoursePermissions;

namespace backend.Application.Common.Interfaces;

public interface ICourseService
{
    // Enrollment management
    Task<CourseEnrollment> EnrollUserInCourseAsync(ApplicationUser user, CourseId courseId, string inviteToken);
    Task UnenrollUserFromCourseAsync(ApplicationUser user, CourseId courseId);
    Task<ICollection<CourseEnrollment>> GetCourseEnrollmentsAsync(CourseId courseId);

    // Course management
    Task<ICollection<CoursePermissionType>> GetUserCoursePermissionsAsync(CourseId courseId);
    Task<ICollection<Course>> ListCoursesAsync();
    Task<Course> GetCourseDetailsAsync(CourseId courseId);
    Task<Course> CreateCourseAsync(ApplicationUser owner, string internalIdentifier, string name, string description, Season semesterSeason, int semesterYear);
    Task<Course> UpdateCourseAsync(CourseId courseId, string name, string description);
    Task DeleteCourseAsync(CourseId courseId);

    // Invite link management
    Task<CourseInviteLink> GenerateCourseInviteLinkAsync(CourseId courseId, string? title = null, DateTimeOffset? expirationDate = null);
    Task RevokeCourseInviteLinkAsync(CourseId courseId, CourseInviteLinkId inviteLinkId);
    Task<ICollection<CourseInviteLink>> ListCourseInviteLinksAsync(CourseId courseId);
    Task<CourseInviteLink> GetCourseInviteLinkDetailsAsync(CourseId courseId, CourseInviteLinkId inviteLinkId);

    // Instructor management
    Task AddInstructorToCourseAsync(ApplicationUser user, CourseId courseId);
    Task RemoveInstructorFromCourseAsync(ApplicationUser user, CourseId courseId);
    Task<ICollection<CourseInstructor>> ListCourseInstructorsAsync(CourseId courseId);

    // Lecture management
    Task<ICollection<Lecture>> GetCourseLecturesAsync(CourseId courseId);
    Task<LectureContent> GetLectureContentDetailsAsync(CourseId courseId, LectureId lectureId, LectureContentId lectureContentId);
    Task<Lecture> GetLectureDetailsAsync(CourseId courseId, LectureId lectureId);
    Task<LectureContent> UploadLectureMaterialAsync(CourseId courseId, LectureId lectureId, IFormFile file, LectureContentType contentType, string name, string? description, bool isMainContent = false, CancellationToken cancellationToken = default);
    Task<Lecture> CreateLectureAsync(CourseId courseId, string title, string description, DateTimeOffset startDate, DateTimeOffset endDate);
    Task<IFormFile> GetLectureContentStreamAsync(CourseId courseId, LectureId lectureId, LectureContentId lectureContentId, ResourceId resourceId, CancellationToken cancellationToken = default);
    Task<Lecture> UpdateLectureAsync(CourseId courseId, LectureId lectureId, string title, string content);
    Task DeleteLectureAsync(CourseId courseId, LectureId lectureId);

}
