namespace backend.Infrastructure.MediaProcessing.Configurations;
public class TranscriptionConfiguration
{
    public string? LocalTranscriptionApiHost { get; set; }
    public string? AzureTranscriptionApiKey { get; set; }
    public string? AzureTranscriptionRegion { get; set; }
}