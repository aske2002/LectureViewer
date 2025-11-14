namespace backend.Domain.Identifiers;

public class FlashcardId : StronglyTypedId<FlashcardId>
{
    public FlashcardId(Guid value) : base(value) {}
}
