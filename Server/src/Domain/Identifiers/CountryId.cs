namespace backend.Domain.Identifiers;

public class CountryId : StronglyTypedId<CountryId>
{
    public CountryId(Guid value) : base(value) {}
}