using System.Text.Json.Serialization;

namespace backend.Application.Common.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleDto
    {
        Administrator,
        Instructor
    }
}