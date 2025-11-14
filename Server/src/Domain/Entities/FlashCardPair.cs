using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class FlashcardPair : BaseAuditableEntity<FlashcardPairId>
{
    public required FlashcardChoiceId KeyChoiceId { get; set; }
    public required FlashcardChoice KeyChoice { get; set; }
    public required FlashcardChoiceId ValueChoiceId { get; set; }
    public required FlashcardChoice ValueChoice { get; set; }

    public required FlashcardId FlashcardId { get; set; }
    public required MatchingFlashcard Flashcard { get; set; }
}

public class FlashcardPairAnswer : BaseAuditableEntity<FlashcardPairAnswerId>
{
    public required FlashcardChoiceId SelectedKeyId { get; set; }
    public required FlashcardChoice SelectedKey { get; set; }
    public required FlashcardChoiceId SelectedValueId { get; set; }
    public required FlashcardChoice SelectedValue { get; set; }
    
    public required FlashcardAnswerId FlashcardAnswerId { get; set; }
    public required MatchingFlashcardAnswer FlashcardAnswer { get; set; }
}