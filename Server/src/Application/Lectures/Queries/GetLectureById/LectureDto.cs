using AutoMapper;
using backend.Application.Common.Models;
using backend.Application.Countries.Queries.GetCountries;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.ValueObjects;

namespace backend.Application.Lectures.Queries.GetLectureById;

public record LectureDto : BaseResponse<LectureId>
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required IList<LectureContentDto> Contents { get; init; } = new List<LectureContentDto>();
    public LectureContentDto? PrimaryContent => Contents.FirstOrDefault(c =>
        (
            c.ContentType == Domain.Enums.LectureContentType.Video ||
            c.ContentType == Domain.Enums.LectureContentType.Audio) &&
        c.IsMainContent &&
        c.Resource != null);
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Lecture, LectureDto>();
        }
    }
}

