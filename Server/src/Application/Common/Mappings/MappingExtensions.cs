using backend.Application.Common.Models;

namespace backend.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<FilteredList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => FilteredList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, null);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
}
