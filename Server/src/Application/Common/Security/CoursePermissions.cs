using System.Text.Json.Serialization;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Authorization;

namespace backend.Domain.Constants;


public static class CoursePermissions
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CoursePermissionType
    {
        UploadMedia,
        Delete,
        CreateLectures,
        Edit,
        View
    }

    public class UploadMedia : IAuthorizationRequirement { }
    public class Delete : IAuthorizationRequirement { }
    public class CreateLectures : IAuthorizationRequirement { }
    public class Edit : IAuthorizationRequirement { }
    public class View : IAuthorizationRequirement { }
    public static CoursePermissionType ToCoursePermission(IAuthorizationRequirement requirement)
    {
        return requirement switch
        {
            UploadMedia => CoursePermissionType.UploadMedia,
            Delete => CoursePermissionType.Delete,
            CreateLectures => CoursePermissionType.CreateLectures,
            Edit => CoursePermissionType.Edit,
            View => CoursePermissionType.View,
            _ => throw new ArgumentOutOfRangeException(nameof(requirement), "Unknown permission requirement")
        };
    }

    public static IAuthorizationRequirement FromCoursePermission(CoursePermissionType permission)
    {
        return permission switch
        {
            CoursePermissionType.UploadMedia => new UploadMedia(),
            CoursePermissionType.Delete => new Delete(),
            CoursePermissionType.CreateLectures => new CreateLectures(),
            CoursePermissionType.Edit => new Edit(),
            CoursePermissionType.View => new View(),
            _ => throw new ArgumentOutOfRangeException(nameof(permission), "Unknown permission type")
        };
    }
}