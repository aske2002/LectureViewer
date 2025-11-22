namespace backend.Domain.Identifiers;

public class TranscriptItemId : StronglyTypedId<TranscriptItemId>
{
    public TranscriptItemId(Guid value) : base(value) {}
}
