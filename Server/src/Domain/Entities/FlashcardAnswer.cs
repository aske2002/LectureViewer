using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public abstract class FlashcardAnswer : BaseAuditableEntity<FlashcardAnswerId>
{
    public required FlashcardType FlashcardType { get; set; }
    public required FlashcardId FlashcardId { get; set; }
    public required Flashcard Flashcard { get; set; }
    public required CourseEnrollmentId CourseEnrollmentId { get; set; }
    public required CourseEnrollment CourseEnrollment { get; set; }
}

public class SingleChoiceFlashcardAnswer : FlashcardAnswer
{
    public required FlashcardChoiceId SelectedChoiceId { get; set; }
    public required FlashcardChoice SelectedChoice { get; set; }
}

public class MultipleChoiceFlashcardAnswer : FlashcardAnswer
{
    public required IList<FlashcardChoice> SelectedChoices { get; set; }
}

public class MatchingFlashcardAnswer : FlashcardAnswer
{
    public required IList<FlashcardPairAnswer> PairAnswers { get; set; }
}

public class FreeTextFlashcardAnswer : FlashcardAnswer
{
    public required string AnswerText { get; set; }
}

public class TrueFalseFlashcardAnswer : FlashcardAnswer
{
    public required bool AnswerBool { get; set; }
}

