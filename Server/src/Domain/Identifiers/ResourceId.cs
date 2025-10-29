namespace backend.Domain.Identifiers;

public class ResourceId : StronglyTypedId<ResourceId>
{
    public ResourceId(Guid value) : base(value) {}
}