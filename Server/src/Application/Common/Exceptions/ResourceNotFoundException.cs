
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class ResourceNotFoundException : NotFoundExceptionBase<ResourceId, ResourceNotFoundException>
{
    public override string EntityName => nameof(Country);
    public ResourceNotFoundException(ResourceId resourceId)
        : base(resourceId) { }
}