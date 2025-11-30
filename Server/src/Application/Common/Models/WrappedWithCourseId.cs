using backend.Domain.Identifiers;

public record WithCourseId<T, R> : IRequest<R>
{
    public required CourseId CourseId { get; set; }
    public required T Command { get; set; }
}