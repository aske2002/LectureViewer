using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class CourseCategory : BaseAuditableEntity<CourseCategoryId>
{
    public required string Question { get; set; }
    public required string Answer { get; set; }
    public required LectureId LectureId { get; set; }
    public required Lecture Lecture { get; set; }
    public IList<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}