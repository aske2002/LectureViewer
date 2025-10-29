namespace backend.Domain.Identifiers;

public class CourseEnrollmentId : StronglyTypedId<CourseEnrollmentId>
{
    public CourseEnrollmentId(Guid value) : base(value) {}
}
