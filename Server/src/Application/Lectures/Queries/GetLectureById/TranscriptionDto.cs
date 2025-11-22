using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;

namespace backend.Application.Lectures.Queries.GetLectureById;

public record TranscriptionItemDto : BaseResponse<TranscriptItemId>
{
    public required TimeSpan StartTime { get; set; }
    public required TimeSpan EndTime { get; set; }
    public required string Text { get; set; }
}

public record TranscriptionDto : BaseResponse<TranscriptId>
{
    public IList<TranscriptionItemDto> Items { get; set; } = new List<TranscriptionItemDto>();
    public string? Summary { get; set; }
    public required string Language { get; set; }
    public required string TranscriptText { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Transcript, TranscriptionDto>();
        }
    }
}

