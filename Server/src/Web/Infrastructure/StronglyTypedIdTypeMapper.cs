using backend.Domain.Common;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using NSwag.Generation;

namespace backend.Web.Infrastructure;
public class StronglyTypedIdTypeMapper<TId> : ITypeMapper
    where TId : StronglyTypedId<TId>
{
    public Type MappedType => typeof(TId);

    public bool UseReference => false;

    public void GenerateSchema(JsonSchema schema, TypeMapperContext context)
    {
        schema.Type = JsonObjectType.String;
        schema.Format = "uuid";
        schema.IsNullableRaw = false;
        schema.Description = $"A {typeof(TId).Name} identifier";
        schema.Example = Guid.NewGuid().ToString();
    }
}

public static class StronglyTypedIdTypeMapperExtensions
{
    public static void AddStronglyTypedIdTypeMapper(this OpenApiDocumentGeneratorSettings settings)
    {
        var allStronglyTypedIds = typeof(StronglyTypedId<>).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(StronglyTypedId<>))
            .ToList();
        foreach (var stronglyTypedId in allStronglyTypedIds)
        {
            var genericType = stronglyTypedId.BaseType?.GetGenericArguments()[0];
            if (genericType == null)
                continue;

            var mapperType = typeof(StronglyTypedIdTypeMapper<>).MakeGenericType(genericType);
            var mapperInstance = Activator.CreateInstance(mapperType);
            if (mapperInstance == null)
                continue;

            settings.SchemaSettings.TypeMappers.Add((ITypeMapper)mapperInstance);
        }
    }
}