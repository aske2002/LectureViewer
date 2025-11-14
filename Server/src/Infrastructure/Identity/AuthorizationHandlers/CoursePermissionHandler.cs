using backend.Application.Common.Interfaces;
using backend.Domain.Constants;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace backend.Infrastructure.Identity.AuthorizationHandlers;

public class CoursePermissionHandler : IAuthorizationHandler
{
    private readonly IUserAccessor _userAccessor;

    public CoursePermissionHandler(IUserAccessor userAccessor)
    {
        _userAccessor = userAccessor;
    }

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        var pendingRequirements = context.PendingRequirements.ToList();
        var user = await _userAccessor.GetCurrentUserAsync();

        if (context.Resource is not Course course || user == null)
        {
            return;
        }

        foreach (var requirement in pendingRequirements)
        {

            if (requirement is CoursePermissions.CreateLectures || requirement is CoursePermissions.UploadMedia || requirement is CoursePermissions.Delete || requirement is CoursePermissions.Edit)
            {
                if (IsInstructor(user, course))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement is CoursePermissions.View)
            {
                if (IsInstructor(user, course) || IsStudent(user, course))
                {
                    context.Succeed(requirement);
                }
            }
        }
    }

    private static bool IsInstructor(ApplicationUser user, Course resource)
    {
        return resource.Instructors.Any(i => i.InstructorId == user.Id);
    }

    private static bool IsStudent(ApplicationUser user, Course resource)
    {
        return resource.Enrollments.Any(e => e.User.Id == user.Id);
    }
}