namespace backend.Domain.Identifiers;

public class SemesterId : StronglyTypedId<SemesterId>
{
    public SemesterId(Guid value) : base(value) {}
}
