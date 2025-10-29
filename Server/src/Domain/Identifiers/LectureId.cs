namespace backend.Domain.Identifiers;

public class LectureId : StronglyTypedId<LectureId>
{
    public LectureId(Guid value) : base(value) {}
}
