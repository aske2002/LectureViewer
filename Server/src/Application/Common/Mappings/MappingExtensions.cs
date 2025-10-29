using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper.Internal;
using backend.Application.Common.Attributes;
using backend.Application.Common.Interfaces;
using backend.Application.Common.Models;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Common;
using backend.Domain.Entities;
using backend.Domain.Extensions;
using backend.Domain.Identifiers;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace backend.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<FilteredList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => FilteredList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, null);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();


    public static async Task<List<TProject>> MapResourcesAsync<TProject, T, TId>(IApplicationDbContext context, IMapper mapper, IQueryable<T> query) where T : BaseEntity<TId>
        where TId : StronglyTypedId<TId>
        where TProject : BaseResponse<TId>
    {
        var includes = new List<string?>() { "" };
        includes.AddRange(query.GetIncludes());

        var entityIdGetters = includes.SelectMany(ResourceAccessorCollector.CollectAllResourceAccessorsAlongPath<TProject>);
        var entities = await query.ProjectToListAsync<TProject>(mapper.ConfigurationProvider);

        var entityIds = entities.SelectMany(e => entityIdGetters.Select(g => g.idGetter(e)))
            .ToList()
            .ToHashSet()
            .ToList();

        var resources = await context.Resources
            .Where(r => entityIds.Contains(r.EntityId))
            .ToListAsync();

        foreach (var idGetter in entityIdGetters)
        {
            var resourceType = idGetter.type.GetCustomAttribute<ResourceResponseAttribute>()?.ResourceType;
            var propertyType = idGetter.type.PropertyType;

            foreach (var entity in entities)
            {
                var id = idGetter.idGetter(entity);
                var entityResources = resources.Where(r => r.EntityId == id).ToList();
                if (propertyType == typeof(ResourceResponse))
                {
                    Resource? resource;
                    if (resourceType == null)
                    {
                        resource = entityResources.FirstOrDefault();
                    }
                    else
                    {
                        resource = entityResources.FirstOrDefault(r => r.ResourceType == resourceType);
                    }

                    if (Nullable.GetUnderlyingType(propertyType) == null && resource == null)
                    {
                        Debug.WriteLine($"Resource not found for entity {id} and property {idGetter.type.Name}");
                    }
                    else
                    {
                        idGetter.resourceSetter(entity, mapper.Map<ResourceResponse>(resource));
                    }

                }
            }
        }

        return entities;
    }
}
