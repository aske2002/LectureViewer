using AutoMapper;
using backend.Application.Common.Models;
using backend.Application.Countries.Queries.GetCountries;
using backend.Application.LectureContents.Queries.ListLectureContents;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.ValueObjects;

namespace backend.Application.Lectures.Queries.GetLectureById;

public record LecturePrimaryResourceDto : BaseResponse<LectureId>
{
    public LectureContentDto? Presentation { get; set; }
    public TranscriptionDto? Transcription { get; set; }
    public LectureContentDto? Media { get; set; }
}

public record LectureDto : BaseResponse<LectureId>
{
    public required CourseId CourseId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public LecturePrimaryResourceDto? PrimaryResource { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Lecture, LectureDto>();
        }
    }
}

