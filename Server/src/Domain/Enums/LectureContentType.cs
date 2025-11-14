using System.Text.Json.Serialization;

namespace backend.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LectureContentType
{
    Video = 0,
    Audio = 1,
    Slides = 2,
    Document = 3,
    Other = 4
}
