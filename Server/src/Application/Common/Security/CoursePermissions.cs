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
        Delete,
        CreateLectures,
        UploadCourseContent,
        Edit,
        View
    }

    public class CoursePermission : IAuthorizationRequirement
    {
        public CoursePermissionType[] Permissions { get; }
        public CoursePermission(params CoursePermissionType[] permissionType)
        {
            Permissions = permissionType;
        }
    }
}