using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Domain.Identifiers;

namespace backend.Application.Resources.Queries.GetResourceById;
public record ResourceResponse : BaseResponse<ResourceId>
{
    public ResourceType ResourceType { get; init; } = ResourceType.Empty;
    public string FileName { get; init; } = string.Empty;
    public string MimeType { get; init; } = string.Empty;
    public string Url { get => $"/api/Resources/{Id}/{FileName}"; }
    public int Size { get; init; } = 0;
    public int? Order { get; init; } = 0;
}
