
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class ResourceContentNotFoundException : NotFoundExceptionBase<ResourceId, ResourceContentNotFoundException>
{
    public override string EntityName => "Resource content";
    public ResourceContentNotFoundException(ResourceId resourceId)
        : base(resourceId)
    {

    }
}