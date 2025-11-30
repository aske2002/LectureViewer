using backend.Application.Common.Interfaces;
using backend.Domain.Constants;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using static backend.Domain.Constants.CoursePermissions;

namespace backend.Infrastructure.Identity.AuthorizationHandlers;

public class CoursePermissionHandler : AuthorizationHandler<CoursePermission>
{
    private readonly IUserAccessor _userAccessor;
    private readonly ICourseService _courseService;

    public CoursePermissionHandler(IUserAccessor userAccessor, ICourseService courseService)
    {
        _userAccessor = userAccessor;
        _courseService = courseService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CoursePermission requirement)
    {
        if (context.Resource is HttpContext httpContext)
        {
            if (CourseId.TryParse(httpContext.GetRouteValue("courseId")?.ToString() ?? string.Empty, null, id: out var courseId))
            {
                var user = await _userAccessor.GetCurrentUserAsync();
                var course = await _courseService.GetUserCoursePermissionsAsync(courseId, user);

                if (requirement.Permissions.All(p => course.Contains(p)))
                {
                    context.Succeed(requirement);
                }
                return;
            }
        }
    }
}