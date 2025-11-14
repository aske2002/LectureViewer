namespace backend.Domain.Entities;

using backend.Domain.Enums;
using backend.Domain.Identifiers;

public class Resource : BaseAuditableEntity<ResourceId>
{
    public ResourceType ResourceType { get; init; }
    public IList<Resource> AssociatedResources { get; private set; } = new List<Resource>();
    public ResourceId? ParentResourceId { get; set; }
    public Resource? ParentResource { get; set; }
    public required string FileName { get; init; }
    public required string MimeType { get; init; }
    public int Size { get; init; }
    public int? Order { get; init; }
}