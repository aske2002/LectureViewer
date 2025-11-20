using System.Text.Json;
using System.Text.Json.Serialization;

public class CamelCaseEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value == null)
            throw new JsonException();

        // Convert "log" → "Log"
        var pascal = char.ToUpper(value[0]) + value.Substring(1);

        if (Enum.TryParse<T>(pascal, out var result))
            return result;

        throw new JsonException($"Unable to convert \"{value}\" to {typeof(T)}.");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        // Convert "Log" → "log"
        var camel = char.ToLower(value.ToString()[0]) + value.ToString().Substring(1);
        writer.WriteStringValue(camel);
    }
}
