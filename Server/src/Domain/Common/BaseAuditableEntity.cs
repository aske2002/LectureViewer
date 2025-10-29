namespace backend.Domain.Common;

public interface IBaseAuditableEntity
{
    DateTimeOffset Created { get; set; }
    string? CreatedBy { get; set; }
    DateTimeOffset LastModified { get; set; }
    string? LastModifiedBy { get; set; }
}

public abstract class BaseAuditableEntity<TId> : BaseEntity<TId>, IBaseAuditableEntity
    where TId : StronglyTypedId<TId>
{
    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}
