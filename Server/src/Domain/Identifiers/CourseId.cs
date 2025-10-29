namespace backend.Domain.Identifiers;

public class CourseId : StronglyTypedId<CourseId>
{
    public CourseId(Guid value) : base(value) {}
}
