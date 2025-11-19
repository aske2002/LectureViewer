using System.Net.Http.Headers;
using System.Net.Http.Json;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Infrastructure.MediaProcessing.Transcription.Models;
using backend.Infrastructure.Models;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.MediaProcessing.Transcription;

public class LocalTranscriptionService : ITranscriptionService
{
    private readonly HttpClient httpClient;
    private readonly IResourceService _resourceService;
    private readonly ILogger<LocalTranscriptionService> _logger;

    public LocalTranscriptionService(HttpClient httpClient, IResourceService resourceService, ILogger<LocalTranscriptionService> logger)
    {
        this.httpClient = httpClient;
        _resourceService = resourceService;
        _logger = logger;
    }

    static readonly string HealthCheckEndpoint = "/health";
    static readonly string ModelsEndpoint = "/models";
    static readonly string LanguagesEndpoint = "/languages";
    static readonly string TranscriptionsEndpoint = "/transcriptions";
    static readonly string TranscriptionEndpoint = "/transcriptions/{id}";
    static readonly string StreamTranscriptionEndpoint = "/transcriptions/{id}/stream";

    private async Task<bool> IsServiceHealthyAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(HealthCheckEndpoint, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    private async Task<string> GetAvailableModelsAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(ModelsEndpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    private async Task<string> GetAvailableLanguagesAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(LanguagesEndpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    private async Task<List<string>> ListTranscriptionsAsync(string id, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(TranscriptionsEndpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<string>>(cancellationToken: cancellationToken) ?? new List<string>();
    }

    private async Task<string> StartTranscriptionAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
    {
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);

        var response = await httpClient.PostAsync(TranscriptionsEndpoint, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    private async Task<WhisperProcessResult> GetTranscriptionAsync(string id, CancellationToken cancellationToken)
    {
        var endpoint = TranscriptionEndpoint.Replace("{id}", id);
        var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<WhisperProcessResult>(cancellationToken: cancellationToken);
        if (result == null)
        {
            throw new Exception("Failed to deserialize transcription result.");
        }
        return result;
    }

    private async Task<IAsyncEnumerable<WhisperEvent?>> StreamTranscriptionAsync(string id, CancellationToken cancellationToken)
    {
        var endpoint = StreamTranscriptionEndpoint.Replace("{id}", id);
        var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        var contentStream = response.Content.ReadFromJsonAsAsyncEnumerable<WhisperEvent?>(cancellationToken: cancellationToken);
        if (contentStream == null)
        {
            throw new Exception("Failed to get transcription stream.");
        }
        return contentStream;
    }

    public async Task<string> TranscribeAsync(Resource file, CancellationToken cancellationToken)
    {
        var resourceContent = await _resourceService.GetResourceStreamByIdAsync(file.Id, cancellationToken);
        var id = await StartTranscriptionAsync(resourceContent, file.FileName, cancellationToken);
        var eventStream = await StreamTranscriptionAsync(id, cancellationToken);
        await foreach (var output in eventStream.WithCancellation(cancellationToken))
        {
            if (output is WhisperEndEvent endEvent)
            {
                var result = await GetTranscriptionAsync(id, cancellationToken);
                if (result is WhisperProcessSuccessResult successResult)
                {
                    return successResult.Data.Transcription.Select(e => e.Text).Aggregate((a, b) => a + b);
                }
                else if (result is WhisperProcessErrorResult errorResult)
                {
                    throw new Exception($"Transcription failed: {errorResult.ErrorMessage}");
                }
                else
                {
                    throw new Exception("Unknown transcription result.");
                }
            } else if (output is WhisperLogEvent logEvent)
            {
                _logger.LogInformation("Transcription log: {Message}", logEvent.Message);
            }
        }
        
        throw new Exception("Transcription did not complete successfully.");
    }
}