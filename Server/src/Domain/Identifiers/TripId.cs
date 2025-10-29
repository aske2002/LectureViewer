namespace backend.Domain.Identifiers;

public class TripId : StronglyTypedId<TripId>
{
    public TripId(Guid value) : base(value) {}
}