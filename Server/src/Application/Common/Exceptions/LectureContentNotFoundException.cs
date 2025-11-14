using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Common.Exceptions;
public class LectureContentNotFoundException : NotFoundExceptionBase<LectureContentId, LectureContentNotFoundException>
{
    public override string EntityName => nameof(LectureContent);
    public LectureContentNotFoundException(LectureContentId lectureContentId)
        : base(lectureContentId) { }
}