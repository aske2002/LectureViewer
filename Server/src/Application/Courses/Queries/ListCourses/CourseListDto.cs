using AutoMapper;
using backend.Application.Common.Models;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Courses.Queries.ListCourses;

public record CourseListDto : BaseResponse<CourseId>
{
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
    public required string InternalIdentifier { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public SemesterDto Semester { get; init; } = null!;
    public required ICollection<PublicUser> Instructors { get; init; } = new List<PublicUser>();

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Course, CourseListDto>()
                .ForMember(dest => dest.Instructors, 
                    opt => opt.MapFrom(src => src.Instructors.Select(ci => ci.Instructor)));
                    
            CreateMap<CourseInstructor, PublicUser>()
                .ConvertUsing(ci => new PublicUser
                {
                    Id = ci.Instructor.Id,
                    UserName = ci.Instructor.UserName,
                    FirstName = ci.Instructor.FirstName,
                    LastName = ci.Instructor.LastName
                });
        }
    }
}