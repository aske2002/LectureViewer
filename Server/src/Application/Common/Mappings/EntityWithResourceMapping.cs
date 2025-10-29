
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.Internal;
using backend.Application.Common.Attributes;
using backend.Application.Common.Interfaces;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Common;
using backend.Domain.Entities;
using backend.Domain.Extensions;
using backend.Domain.Helpers;

namespace backend.Application.Common.Mappings;
public static class CreateResourceMappingExtensions
{
    public class ResourceValueResolver<TSource, TDestination, TId> : IValueResolver<TSource, TDestination, Resource?> where TSource : BaseAuditableEntity<TId>
        where TId : StronglyTypedId<TId>
        where TDestination : BaseResponse<TId>
    {
        private readonly IApplicationDbContext _context;
        public ResourceValueResolver(IApplicationDbContext context)
        {
            _context = context;
        }

        public Resource? Resolve(TSource source, TDestination destination, Resource? destMember, ResolutionContext context)
        {

            var resourceType = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(Resource) || (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) && p.PropertyType.GenericTypeArguments[0] == typeof(Resource)))
                .Select(p => p.GetCustomAttribute<ResourceResponseAttribute>()?.ResourceType)
                .FirstOrDefault();

            return resourceType == null
                ? _context.Resources.FirstOrDefault(r => r.EntityId == source.Id.Value)
                : _context.Resources.FirstOrDefault(r => r.EntityId == source.Id.Value && r.ResourceType == resourceType);

        }
    }


    public static void CreateResourceMapping<TSource, TDestination, TId>(IMapperConfigurationExpression configuration) where TSource : BaseAuditableEntity<TId>
    where TId : StronglyTypedId<TId>
    where TDestination : BaseResponse<TId>
    {

        var destinationMembers = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(ResourceResponse) || (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) && p.PropertyType.GenericTypeArguments[0] == typeof(ResourceResponse)));

        var projection = configuration.CreateMap<TSource, TDestination>();

        foreach (var member in destinationMembers)
        {
            var attribute = member.GetCustomAttribute(typeof(ResourceResponseAttribute), true) as ResourceResponseAttribute;
            var resourceType = attribute?.ResourceType;
            var memberType = member.PropertyType;
            if (attribute == null)
                continue;
            if (memberType == typeof(ResourceResponse))
            {
                var memberExpression = Expression.Lambda<Func<TDestination, ResourceResponse>>(
                    Expression.Property(Expression.Parameter(typeof(TDestination), "src"), member.Name),
                    Expression.Parameter(typeof(TDestination), "src"));
                projection.ForMember(memberExpression, opt => opt.Ignore());
            }
            else if (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var listType = memberType.GenericTypeArguments[0];
                if (listType != typeof(ResourceResponse))
                    continue;

                var memberExpression = Expression.Lambda<Func<TDestination, List<ResourceResponse>>>(
                    Expression.Property(Expression.Parameter(typeof(TDestination), "src"), member.Name),
                    Expression.Parameter(typeof(TDestination), "src"));
                projection.ForMember(memberExpression, opt => opt.Ignore());
            }
        }
    }


    public static void CreateMappersForEntities(

        this IMapperConfigurationExpression configuration
    )
    {
        var entityTypes = typeof(BaseAuditableEntity<>).GetAllDerivedTypes();

        var responseTypes = typeof(BaseResponse<>).GetAllDerivedTypes();

        foreach (var entityType in entityTypes)
        {
            var entityIdType = entityType.DomainGenericParamsOnBase[0];
            if (entityIdType == null)
                continue;

            foreach (var responseType in responseTypes)
            {
                var responseIdType = responseType.DomainGenericParamsOnBase[0];

                if (responseIdType == null || !entityIdType.IsAssignableFrom(responseIdType))
                    continue;

                var mapMethod = typeof(CreateResourceMappingExtensions)
                    .GetMethod(nameof(CreateResourceMapping), BindingFlags.Static | BindingFlags.Public)
                    ?.MakeGenericMethod(entityType.DomainType, responseType.DomainType, entityIdType);

                mapMethod?.Invoke(null, [configuration]);
            }
        }
    }
}
