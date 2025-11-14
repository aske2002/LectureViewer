using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;

public class LectureHasNoPrimaryMediaException : NotFoundExceptionBase<LectureId, LectureHasNoPrimaryMediaException>
{
    public LectureHasNoPrimaryMediaException(LectureId lectureId)
        : base(lectureId, $"Lecture with ID {lectureId} has no primary media to display.") { }

    
    public override string EntityName => nameof(Lecture);
}