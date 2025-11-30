using backend.Application.Courses.Commands.CreateCourse;
using backend.Application.Courses.Commands.CreateLecture;
using backend.Application.Courses.Queries.GetCourseById;
using backend.Application.Courses.Queries.ListCourses;
using backend.Domain.Constants;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Web.Endpoints;

public class Courses : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(ListCourses)
            .MapPost(EnrollInCourse, "/enroll")
            .MapGet(GetCourseById, "/{courseId}")
            .MapGet(GetCoursePermissions, "/{courseId}/permissions")
            .MapPost(CreateCourse)
            .MapPost(AddLectureToCourse, "/{courseId}/lectures");
    }
    
    [Authorize(Policy = Policies.ViewCourse)]
    public async Task<Ok<CourseDetailsDto>> GetCourseById(ISender sender, CourseId courseId)
    {
        var vm = await sender.Send(new GetCourseQuery(courseId));
        return TypedResults.Ok(vm);
    }
    public async Task<Ok<List<CourseListDto>>> ListCourses(ISender sender)
    {
        var vm = await sender.Send(new ListCoursesWithPaginationQuery());
        return TypedResults.Ok(vm);
    }

    
    public async Task<Ok<CourseEnrollmentId>> EnrollInCourse(ISender sender, EnrollCourseCommand command)
    {
        var vm = await sender.Send(command);
        return TypedResults.Ok(vm);
    }

    [Authorize(Policy = Policies.CanCreateCourses)]
    public async Task<Ok<CourseId>> CreateCourse(ISender sender, CreateCourseCommand command)
    {
        var vm = await sender.Send(command);
        return TypedResults.Ok(vm);
    }

    [Authorize(Policy = Policies.EditCourse)]
    public async Task<Ok<LectureId>> AddLectureToCourse(ISender sender, CourseId courseId, CreateLectureCommand command)
    {
        var vm = await sender.Send(command);
        return TypedResults.Ok(vm);
    }

    [Authorize(Policy = Policies.ViewCourse)]
    public async Task<Ok<CoursePermissionsDto>> GetCoursePermissions(ISender sender, CourseId courseId)
    {
        var vm = await sender.Send(new GetCoursePermissionsQuery(courseId));
        return TypedResults.Ok(vm);
    }
}
