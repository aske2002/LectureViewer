using AutoMapper;
using backend.Application.Common.Models;
using backend.Application.Countries.Queries.GetCountries;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.ValueObjects;

namespace backend.Application.Courses.Queries.GetCourseById;

public record CourseDetailsDto : BaseResponse<CourseId>
{
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public SemesterDto Semester { get; init; } = null!;
    public required Colour Colour { get; init; } = Colour.Blue;
    public required string InternalIdentifier { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required ICollection<LectureSimpleDto> Lectures { get; init; } = new List<LectureSimpleDto>();
    public required ICollection<PublicUser> Instructors { get; init; } = new List<PublicUser>();
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Course, CourseDetailsDto>()
            .ForMember(dest => dest.Instructors, opt => opt.MapFrom(src => src.Instructors.Select(ci => ci.Instructor)))
            .ForMember(dest => dest.Lectures, opt => opt.MapFrom(src => src.Lectures));
        }
    }
}

