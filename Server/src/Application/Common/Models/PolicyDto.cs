using System.Text.Json.Serialization;

namespace backend.Contracts.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PolicyDto
    {
        CanCreateCourses,
        CanEditCourses,
        CanDeleteCourses
    }
}