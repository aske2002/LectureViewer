using System.Security.Claims;
using backend.Application.Common.Exceptions;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Models;
using backend.Domain.Constants;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Services;

public class CourseService : ICourseService
{
    private readonly IIdentityService _identityService;
    private readonly IResourceService _resourceService;
    private readonly IRepository<Course, CourseId> _courseRepository;
    private readonly IRepository<CourseEnrollment, CourseEnrollmentId> _enrollmentRepository;
    private readonly IRepository<CourseInviteLink, CourseInviteLinkId> _inviteLinkRepository;
    private readonly IRepository<Lecture, LectureId> _lectureRepository;
    private readonly IRepository<Semester, SemesterId> _semesterRepository;

    public CourseService(
        IIdentityService identityService,
        IResourceService resourceService,
        IRepository<Course, CourseId> courseRepository,
        IRepository<CourseEnrollment, CourseEnrollmentId> enrollmentRepository,
        IRepository<CourseInviteLink, CourseInviteLinkId> inviteLinkRepository,
        IRepository<Lecture, LectureId> lectureRepository,
        IRepository<Semester, SemesterId> semesterRepository)
    {
        _identityService = identityService;
        _resourceService = resourceService;
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _inviteLinkRepository = inviteLinkRepository;
        _lectureRepository = lectureRepository;
        _semesterRepository = semesterRepository;
    }

    public async Task AddInstructorToCourseAsync(ApplicationUser user, CourseId courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        if (course.Instructors.Any(i => i.InstructorId == user.Id))
        {
            throw new InstructorAlreadyPresentException(user, courseId);
        }

        course.Instructors.Add(new CourseInstructor
        {
            CourseId = courseId,
            Course = course,
            InstructorId = user.Id,
            Instructor = user
        });
        await _courseRepository.UpdateAsync(course);
    }

    public async Task<Lecture> CreateLectureAsync(CourseId courseId, string title, string content, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        var lecture = new Lecture
        {
            CourseId = courseId,
            Title = title,
            Description = content,
            Course = course,
            StartDate = startDate,
            EndDate = endDate
        };

        await _lectureRepository.AddAsync(lecture);
        return lecture;
    }

    public async Task DeleteCourseAsync(CourseId courseId)
    {
        await _courseRepository.RemoveAsync(courseId);
    }

    public async Task DeleteLectureAsync(CourseId courseId, LectureId lectureId)
    {
        await _lectureRepository.RemoveAsync(lectureId);
    }

    public async Task<CourseEnrollment> EnrollUserInCourseAsync(ApplicationUser user, CourseId courseId, string inviteToken)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }
        var inviteLink = await _inviteLinkRepository.Query().FirstOrDefaultAsync(link => link.CourseId == courseId && link.Token == inviteToken);
        if (inviteLink == null || inviteLink.ExpirationDate < DateTimeOffset.UtcNow)
        {
            throw new InvalidInviteLinkTokenException(course);
        }

        var existingEnrollment = await _enrollmentRepository.Query().FirstOrDefaultAsync(enrollment => enrollment.CourseId == courseId && enrollment.User.Id == user.Id);
        if (existingEnrollment != null)
        {
            throw new UserAlreadyEnrolledException(user, courseId);
        }

        var enrollment = new CourseEnrollment
        {
            CourseId = courseId,
            User = user,
            Course = course
        };

        await _enrollmentRepository.AddAsync(enrollment);
        return enrollment;
    }

    public async Task<CourseInviteLink> GenerateCourseInviteLinkAsync(CourseId courseId, string? title = null, DateTimeOffset? expirationDate = null)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        var inviteLink = new CourseInviteLink
        {
            CourseId = courseId,
            Course = course,
            Token = Guid.NewGuid().ToString(),
            ExpirationDate = expirationDate ?? DateTimeOffset.UtcNow.AddDays(7),
            Title = title
        };

        await _inviteLinkRepository.AddAsync(inviteLink);
        return inviteLink;
    }

    public async Task<Course> CreateCourseAsync(ApplicationUser owner, string internalIdentifier, string name, string description, Season semesterSeason, int semesterYear)
    {
        var existingSemesterId = await _semesterRepository.QueryAsync(
            filter: s => s.Where(sem => sem.Season == semesterSeason && sem.Year == semesterYear))
            .ContinueWith(t => t.Result.Entities.FirstOrDefault()?.Id);

        if (existingSemesterId is null)
        {
            existingSemesterId = await _semesterRepository.AddAsync(new Semester
            {
                Season = semesterSeason,
                Year = semesterYear
            });
        }

        var course = new Course
        {
            Name = name,
            Description = description,
            SemesterId = existingSemesterId,
            InternalIdentifier = internalIdentifier
        };
        course.Instructors.Add(new CourseInstructor
        {
            Course = course,
            CourseId = course.Id,
            Instructor = owner,
            InstructorId = owner.Id
        });

        await _courseRepository.AddAsync(course);
        return course;
    }

    public async Task<Course> GetCourseDetailsAsync(CourseId courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }
        return course;
    }

    public async Task<ICollection<CourseEnrollment>> GetCourseEnrollmentsAsync(CourseId courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        return await _enrollmentRepository.Query()
            .Where(enrollment => enrollment.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<CourseInviteLink> GetCourseInviteLinkDetailsAsync(CourseId courseId, CourseInviteLinkId inviteLinkId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        var inviteLink = await _inviteLinkRepository.Query()
            .FirstOrDefaultAsync(link => link.CourseId == courseId && link.Id == inviteLinkId);
        if (inviteLink == null)
        {
            throw new CourseInviteLinkNotFoundException(inviteLinkId);
        }

        return inviteLink;
    }

    public async Task<ICollection<Lecture>> GetCourseLecturesAsync(CourseId courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        return await _lectureRepository.Query()
            .Where(lecture => lecture.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<Lecture> GetLectureDetailsAsync(CourseId courseId, LectureId lectureId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        var lecture = await _lectureRepository.GetByIdAsync(lectureId);
        if (lecture == null || lecture.CourseId != courseId)
        {
            throw new LectureNotFoundException(lectureId);
        }

        return lecture;
    }

    public async Task<ICollection<CourseInstructor>> ListCourseInstructorsAsync(CourseId courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId, include: c => c.Include(c => c.Instructors));
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        return course.Instructors;
    }

    public async Task<ICollection<CourseInviteLink>> ListCourseInviteLinksAsync(CourseId courseId)
    {
        return await _inviteLinkRepository.Query()
            .Where(link => link.CourseId == courseId)
            .ToListAsync();
    }

    public async Task RemoveInstructorFromCourseAsync(ApplicationUser user, CourseId courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId, include: c => c.Include(c => c.Instructors));
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        var instructor = course.Instructors.FirstOrDefault(i => i.InstructorId == user.Id);
        if (instructor == null)
        {
            throw new UserNotFoundException(user.Id);
        }

        course.Instructors.Remove(instructor);
        await _courseRepository.UpdateAsync(course);
    }

    public async Task RevokeCourseInviteLinkAsync(CourseId courseId, CourseInviteLinkId inviteLinkId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId, include: c => c.Include(c => c.InviteLinks));
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        var inviteLink = course.InviteLinks.FirstOrDefault(link => link.Id == inviteLinkId);
        if (inviteLink == null)
        {
            throw new CourseInviteLinkNotFoundException(inviteLinkId);
        }

        course.InviteLinks.Remove(inviteLink);
        await _courseRepository.UpdateAsync(course);
    }

    public async Task UnenrollUserFromCourseAsync(ApplicationUser user, CourseId courseId)
    {
        var enrollment = await _enrollmentRepository.Query().FirstOrDefaultAsync(e => e.User.Id == user.Id && e.CourseId == courseId);
        if (enrollment == null)
        {
            throw new UserNotEnrolledException(user.Id, courseId);
        }
        await _enrollmentRepository.RemoveAsync(enrollment);

    }

    public async Task<Course> UpdateCourseAsync(CourseId courseId, string name, string description)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        course.Name = name;
        course.Description = description;
        await _courseRepository.UpdateAsync(course);
        return course;
    }

    public async Task<Lecture> UpdateLectureAsync(CourseId courseId, LectureId lectureId, string title, string description)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        var lecture = await _lectureRepository.GetByIdAsync(lectureId);
        if (lecture == null || lecture.CourseId != courseId)
        {
            throw new LectureNotFoundException(lectureId);
        }

        lecture.Title = title;
        lecture.Description = description;
        await _lectureRepository.UpdateAsync(lecture);
        return lecture;
    }

    public async Task<Resource> UploadLectureMaterialAsync(CourseId courseId, LectureId lectureId, string fileName, ResourceType resourceType, int size, byte[] bytes, string? mimeType = null, int? order = null, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            throw new CourseNotFoundException(courseId);
        }

        var lecture = await _lectureRepository.GetByIdAsync(lectureId);
        if (lecture == null || lecture.CourseId != courseId)
        {
            throw new LectureNotFoundException(lectureId);
        }

        return await _resourceService.CreateResourceAsync(lectureId.Value, fileName, resourceType, size, bytes, mimeType, order, cancellationToken);


    }

    // Implement course-related methods here
}