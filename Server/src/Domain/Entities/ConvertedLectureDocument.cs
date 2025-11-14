using System.ComponentModel.DataAnnotations.Schema;
using backend.Domain.Identifiers;

namespace backend.Domain.Entities;

public class ConvertedLectureDocument : BaseAuditableEntity<ConvertedLectureDocumentId>
{
    
   public required ResourceId ResourceId { get; init; }
   public required LectureContentType ContentType { get; init; }
}