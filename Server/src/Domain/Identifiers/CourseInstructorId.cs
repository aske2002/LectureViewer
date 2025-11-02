namespace backend.Domain.Identifiers;

public class CourseInstructorId : StronglyTypedId<CourseInstructorId>
{
    public CourseInstructorId(Guid value) : base(value) {}
}
