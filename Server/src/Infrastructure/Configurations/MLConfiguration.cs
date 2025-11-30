namespace backend.Infrastructure.Configurations;
public class MLConfiguration()
{
    // string Provider, string ModelName, string? LocalApiHost, string? ApiKey = null
    public required string Provider { get; set; }
    public required string ModelName { get; set; }
    public string? SmallEmbeddingModelName { get; set; }
    public string? EmbeddingModelName { get; set; }
    public string? LocalApiHost { get; set; }
    public string? ApiKey { get; set; }
    
}