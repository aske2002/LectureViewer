using backend.Application.Countries.Queries.GetCountries;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Courses.Queries.GetCourseById;

public record CourseDto : BaseResponse<CourseId>
{
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public SemesterDto Semester { get; init; } = null!;
    public required string InternalIdentifier { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Course, CourseDto>();
        }
    }
}

