using AutoMapper.Configuration.Annotations;
using backend.Application.Countries.Queries.GetCountries;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;

namespace backend.Application.Semesters.Queries.GetSemesterById;

public record SemesterDto : BaseResponse<SemesterId>
{
    public required Season Season { get; init; }
    public required int Year { get; init; }

    [Ignore]
    public DateTimeOffset StartDate => new DateTimeOffset(Year, Season.StartMonth(), 1, 0, 0, 0, TimeSpan.Zero);
    [Ignore]
    public DateTimeOffset EndDate => new DateTimeOffset(Year, Season.EndMonth(), 1, 0, 0, 0, TimeSpan.Zero);

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Semester, SemesterDto>();
        }
    }
}
