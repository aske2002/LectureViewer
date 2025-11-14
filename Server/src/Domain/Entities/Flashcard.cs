using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public abstract class Flashcard : BaseAuditableEntity<FlashcardId>
{
    public MediaProcessingJobId? ArtifactFromJobId { get; set; }
    public MediaProcessingJob? ArtifactFromJob { get; set; }
    public required FlashcardType FlashcardType { get; set; }
    public required string Question { get; set; }
    public required LectureId LectureId { get; set; }
    public required Lecture Lecture { get; set; }
    public required CourseCategoryId ContentCategoryId { get; set; }
    public required CourseCategory ContentCategory { get; set; }
    public IList<FlashcardAnswer> Answers { get; set; } = new List<FlashcardAnswer>();
}

public class SingleChoiceFlashCard : Flashcard
{
    public required IList<FlashcardChoice> Choices { get; set; }
    public required FlashcardChoice CorrectAnswer { get; set; }

}

public class MultipleChoiceFlashCard : Flashcard
{
    public required IList<FlashcardChoice> Choices { get; set; }
    public required IList<FlashcardChoice> CorrectAnswers { get; set; }
}

public class MatchingFlashcard : Flashcard
{
    public required IList<FlashcardPair> Pairs { get; set; }
}

public class FreeTextFlashCard : Flashcard
{
}

public class TrueFalseFlashCard : Flashcard
{
    public required bool CorrectAnswer { get; set; }
}

