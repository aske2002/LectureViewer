namespace backend.Domain.Common;

public record FilteredQuery
{
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; } = 50;
    public string? OrderBy { get; init; }
}
public record FilteredQuery<T> : FilteredQuery
    where T : class
{
    public T? Filter { get; init; }
}
