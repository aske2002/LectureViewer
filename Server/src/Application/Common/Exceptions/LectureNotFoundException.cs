using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class LectureNotFoundException : NotFoundExceptionBase<LectureId, LectureNotFoundException>
{
    public override string EntityName => nameof(Lecture);
    public LectureNotFoundException(LectureId lectureId)
        : base(lectureId) { }
}