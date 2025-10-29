
using backend.Domain.Enums;

namespace backend.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ResourceResponseAttribute : Attribute
{
    public ResourceResponseAttribute(ResourceType resourceType)
    {
        ResourceType = resourceType;
    }

    public ResourceType ResourceType { get; }
}