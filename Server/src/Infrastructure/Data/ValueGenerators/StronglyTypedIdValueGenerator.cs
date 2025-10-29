
using backend.Domain.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

public class StronglyTypedIdValueGenerator<TId> : ValueGenerator<TId>
where TId : StronglyTypedId<TId>
{
    public override bool GeneratesTemporaryValues => false;

    public override TId Next(EntityEntry entry)
    {
        var guid = Guid.NewGuid();
        return (TId)Activator.CreateInstance(typeof(TId), guid)!;
    }
}