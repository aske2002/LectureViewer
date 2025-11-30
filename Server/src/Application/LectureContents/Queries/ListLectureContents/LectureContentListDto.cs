using backend.Domain.Entities;

namespace backend.Application.LectureContents.Queries.ListLectureContents;

public record LectureContentListDto
{
    public IList<LectureContentDto> Contents { get; init; } = new List<LectureContentDto>();
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<List<LectureContent>, LectureContentListDto>()
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src));
        }
    }
}

