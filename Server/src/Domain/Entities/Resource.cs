namespace backend.Domain.Entities;

using backend.Domain.Enums;
using backend.Domain.Identifiers;

public class Resource : BaseAuditableEntity<ResourceId>
{
    public Guid EntityId { get; set; }
    public ResourceType ResourceType { get; init; }
    public required string FileName { get; init; }
    public required string MimeType { get; init; }
    public int Size { get; init; }
    public int? Order { get; init; }
}