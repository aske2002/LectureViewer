using backend.Application.Common.Models;
using backend.Contracts.Enums;
using backend.Domain.Constants;

namespace backend.Application.Common.Mappings;

public static class RolePolicyMapper
{
    // --- Roles ---
    public static RoleDto? ToRoleDto(this string role)
    {
        return role switch
        {
            Roles.Administrator => RoleDto.Administrator,
            Roles.Instructor => RoleDto.Instructor,
            _ => null
        };
    }

    // --- Policies ---
    public static PolicyDto? ToPolicyDto(this string policy)
    {
        return policy switch
        {
            Policies.CanCreateCourses => PolicyDto.CanCreateCourses,
            Policies.DeleteCourse => PolicyDto.DeleteCourse,
            Policies.CreateLectures => PolicyDto.CreateLectures,
            Policies.EditCourse => PolicyDto.EditCourse,
            Policies.ViewCourse => PolicyDto.ViewCourse,
            _ => null
        };
    }

    // Helpers for mapping lists
    public static List<RoleDto> ToRoleDtos(this IEnumerable<string> roles)
        => roles.Select(r => r.ToRoleDto()).Where(r => r != null).Cast<RoleDto>().ToList();

    public static List<PolicyDto> ToPolicyDtos(this IEnumerable<string> policies)
        => policies.Select(p => p.ToPolicyDto()).Where(p => p != null).Cast<PolicyDto>().ToList();
}
