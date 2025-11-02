using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Interfaces;

public interface ICourseService
{
    // Enrollment management
    Task<CourseEnrollment> EnrollUserInCourseAsync(ApplicationUser user, CourseId courseId, string inviteToken);
    Task UnenrollUserFromCourseAsync(ApplicationUser user, CourseId courseId);
    Task<ICollection<CourseEnrollment>> GetCourseEnrollmentsAsync(CourseId courseId);

    // Course management
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
    Task<Lecture> GetLectureDetailsAsync(CourseId courseId, LectureId lectureId);
    Task<Resource> UploadLectureMaterialAsync(CourseId courseId, LectureId lectureId, string fileName, ResourceType resourceType, int size, byte[] bytes, string? mimeType = null, int? order = null, CancellationToken cancellationToken = default);
    Task<Lecture> CreateLectureAsync(CourseId courseId, string title, string content, DateTimeOffset startDate, DateTimeOffset endDate);
    Task<Lecture> UpdateLectureAsync(CourseId courseId, LectureId lectureId, string title, string content);
    Task DeleteLectureAsync(CourseId courseId, LectureId lectureId);

}
