namespace backend.Infrastructure.Models;

public record ResumeExtractionResponse
{
    public required string ResumeText { get; set; }
}