using backend.Application.Common.Models;
using backend.Application.Countries.Queries.GetCountries;
using backend.Application.Courses.Commands.CreateCourse;
using backend.Application.Courses.Queries.GetCourseById;
using backend.Application.Courses.Queries.ListCourses;
using backend.Application.Destinations.Commands.CreateDestination;
using backend.Application.Destinations.Queries.GetDestinations;
using backend.Domain.Identifiers;
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
            .MapPost(CreateCourse);
    }

    public async Task<Ok<CourseEntityDto>> GetCourseById(ISender sender, CourseId courseId)
    {
        var vm = await sender.Send(new GetCourseQuery(courseId));
        return TypedResults.Ok(vm);
    }
    public async Task<Ok<FilteredList<CourseEntityDto>>> ListCourses(ISender sender)
    {
        var vm = await sender.Send(new ListCoursesWithPaginationQuery());
        return TypedResults.Ok(vm);
    }

    public async Task<Ok<CourseEnrollmentId>> EnrollInCourse(ISender sender, EnrollCourseCommand command)
    {
        var vm = await sender.Send(command);
        return TypedResults.Ok(vm);
    }

    public async Task<Ok<CourseId>> CreateCourse(ISender sender, CreateCourseCommand command)
    {
        var vm = await sender.Send(command);
        return TypedResults.Ok(vm);
    }
}
