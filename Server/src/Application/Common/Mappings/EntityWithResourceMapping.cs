using System.Reflection;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Common;
using backend.Domain.Extensions;

namespace backend.Application.Common.Mappings;
public static class CreateResourceMappingExtensions
{
    public static void CreateResourceMapping<TSource, TDestination, TId>(IMapperConfigurationExpression configuration) where TSource : BaseAuditableEntity<TId>
    where TId : StronglyTypedId<TId>
    where TDestination : BaseResponse<TId>
    {

        var destinationMembers = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(ResourceResponse) || (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) && p.PropertyType.GenericTypeArguments[0] == typeof(ResourceResponse)));
        var projection = configuration.CreateMap<TSource, TDestination>();
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
