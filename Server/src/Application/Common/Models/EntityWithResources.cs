using backend.Domain.Common;
using backend.Domain.Entities;

public class EntityWithResources<T, TId> where T : BaseEntity<TId>
where TId : StronglyTypedId<TId>
{
    public required T Entity { get; set; }
    public required IEnumerable<Resource> Resources { get; set; }
}

public class EntityWithResourcesResponse<T, TId> where T : BaseResponse<TId>
where TId : StronglyTypedId<TId>
{
    public required T Entity { get; set; }
    public required IEnumerable<Resource> Resources { get; set; }
}
