using backend.Domain.Common;

namespace backend.Application.Common.Models;

public record FilteredList<T> : FilteredQuery
{
    public IReadOnlyCollection<T> Items { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public FilteredList(IReadOnlyCollection<T> items, int count, int? pageNumber, int? pageSize, string? orderBy)
    {
        OrderBy = orderBy;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = pageSize != null ? (int)Math.Ceiling(count / (double)pageSize) : 0;
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<FilteredList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, string? orderBy)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new FilteredList<T>(items, count, pageNumber, pageSize, orderBy);
    }

    public static FilteredList<T> Create(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize, string? orderBy)
    {
        return new FilteredList<T>(items.ToList(), totalCount, pageNumber, pageSize, orderBy);
    }

    public static FilteredList<T> Create<TEntityRes>(TEntityRes entityRes, string? orderBy = null)
        where TEntityRes : EntitiesResponse<T>
    {
        return new FilteredList<T>(entityRes.Entities, entityRes.TotalCount, entityRes.PageNumber, entityRes.PageSize, orderBy);
    }
}
