namespace backend.Infrastructure.Configurations;
public record MLConfiguration(string Provider, string ModelName, string? LocalApiHost, string? ApiKey = null);