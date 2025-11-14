using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class FlashcardChoice : BaseAuditableEntity<FlashcardChoiceId>
{
    public required string Text { get; set; }
    public required bool IsCorrect { get; set; }

    public FlashcardPair? KeyForFlashcardPair { get; set; }
    public FlashcardPair? ValueForFlashcardPair { get; set; }

    public IList<FlashcardPairAnswer> AnswerKeyForFlashcardPairs { get; set; } = new List<FlashcardPairAnswer>();
    public IList<FlashcardPairAnswer> AnswerValueForFlashcardPairs { get; set; } = new List<FlashcardPairAnswer>();


    public FlashcardId? MultipleChoicecardId { get; set; }
    public MultipleChoiceFlashCard? MultipleChoicecard { get; set; }
    public FlashcardId? CorrectAnswerForMultipleChoicecardId { get; set; }
    public MultipleChoiceFlashCard? CorrectAnswerForMultipleChoicecard { get; set; }
    public IList<MultipleChoiceFlashcardAnswer> AnswersForMultipleChoicecards { get; set; } = new List<MultipleChoiceFlashcardAnswer>();


    public FlashcardId? CorrectAnswerForSingleChoicecardId { get; set; }
    public SingleChoiceFlashCard? CorrectAnswerForSingleChoicecard { get; set; }
    public FlashcardId? SingleChoicecardId { get; set; }
    public SingleChoiceFlashCard? SingleChoiceCard { get; set; }
    public IList<SingleChoiceFlashcardAnswer> AnswerForSinglecards { get; set; } = new List<SingleChoiceFlashcardAnswer>();
    public int? Order { get; set; }
}