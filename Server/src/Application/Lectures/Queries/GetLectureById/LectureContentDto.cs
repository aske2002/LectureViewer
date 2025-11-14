using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;

namespace backend.Application.Lectures.Queries.GetLectureById;

public record LectureContentDto : BaseResponse<LectureContentId>
{

    public required DateTimeOffset Created { get; init; }
    public required ResourceId ResourceId { get; init; }
    public LectureContentType ContentType { get; init; }
    public required ResourceResponse Resource { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsMainContent { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<LectureContent, LectureContentDto>()
                .ForMember(dest => dest.Resource, opt => opt.MapFrom(src => src.Resource));
        }
    }
}

