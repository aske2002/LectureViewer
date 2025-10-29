using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Domain.Helpers;

public class StronglyTypedIdJsonConverterFactory : JsonConverterFactory
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> Cache = new();

    public override bool CanConvert(Type typeToConvert)
    {
        return StronglyTypedIdHelper.IsStronglyTypedId(typeToConvert);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Cache.GetOrAdd(typeToConvert, CreateConverter);
    }

    private static JsonConverter CreateConverter(Type typeToConvert)
    {
        if (!StronglyTypedIdHelper.IsStronglyTypedId(typeToConvert))
            throw new InvalidOperationException($"Cannot create converter for '{typeToConvert}'");

        var type = typeof(StronglyTypedIdJsonConverter<>).MakeGenericType(typeToConvert)
                   ?? throw new InvalidOperationException($"Cannot create converter for '{typeToConvert}'");

        if (Activator.CreateInstance(type) is JsonConverter converter)
            return converter;

        throw new InvalidOperationException($"Cannot create converter for '{typeToConvert}'");
    }
}