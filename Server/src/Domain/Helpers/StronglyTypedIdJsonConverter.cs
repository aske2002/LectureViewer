
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Domain.Helpers;

public class StronglyTypedIdJsonConverter<TStronglyTypedId> : JsonConverter<TStronglyTypedId>
    where TStronglyTypedId : StronglyTypedId<TStronglyTypedId>
{
    public override TStronglyTypedId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
            return null;

        var value = JsonSerializer.Deserialize<Guid>(ref reader, options);
        var factory = StronglyTypedIdHelper.GetFactory(typeToConvert);
        return (TStronglyTypedId)factory(value);
    }

    public override void Write(Utf8JsonWriter writer, TStronglyTypedId value, JsonSerializerOptions options)
    {
        if (value is null)
            writer.WriteNullValue();
        else
            JsonSerializer.Serialize(writer, value.Value, options);
    }
}