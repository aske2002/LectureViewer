using AutoMapper;
using backend.Application.Common.Models;
using backend.Application.Countries.Queries.GetCountries;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.ValueObjects;

namespace backend.Application.Courses.Queries.GetCourseById;

public record LectureSimpleDto : BaseResponse<LectureId>
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Lecture, LectureSimpleDto>();
        }
    }
}

