namespace backend.Domain.Identifiers;

public class CourseCategoryId : StronglyTypedId<CourseCategoryId>
{
    public CourseCategoryId(Guid value) : base(value) {}
}
