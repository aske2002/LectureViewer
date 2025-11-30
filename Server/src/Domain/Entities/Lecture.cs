using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class Lecture : BaseAuditableEntity<LectureId>
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required CourseId CourseId { get; init; }
    public required Course Course { get; init; }
    public IList<LectureContent> Contents { get; private set; } = new List<LectureContent>();
    public IList<Flashcard> Flashcards { get; private set; } = new List<Flashcard>();
}