
using backend.Domain.Common;
using backend.Domain.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace backend.Infrastructure.Data.Extensions;
public static class ChangeTrackerExtensions
{
    public static IEnumerable<EntityEntry<IBaseEntity>> GetAllEntities(this ChangeTracker tracker)
    {
        return tracker
            .Entries<IBaseEntity>()
            .Where(e => e.Entity.GetType().IsBaseEntity());
    }

    public static IEnumerable<EntityEntry<IBaseAuditableEntity>> GetAllAuditableEntities(this ChangeTracker tracker)
    {
        return tracker
            .Entries<IBaseAuditableEntity>()
            .Where(e => e.Entity.GetType().IsBaseAuditableEntity());
    }

}