using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Application.Common.Models;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Infrastructure.MediaProcessing.Transcription.Models;
using backend.Infrastructure.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace backend.Infrastructure.MediaProcessing.Transcription;

public class LocalTranscriptionService : ITranscriptionService
{
    private readonly HttpClient httpClient;
    private readonly IResourceService _resourceService;
    private readonly ILogger<LocalTranscriptionService> _logger;
    private readonly JsonOptions _jsonOptions;

    public LocalTranscriptionService(HttpClient httpClient, IResourceService resourceService, ILogger<LocalTranscriptionService> logger, IOptions<JsonOptions> jsonOptions)
    {
        this.httpClient = httpClient;
        _resourceService = resourceService;
        _logger = logger;
        _jsonOptions = jsonOptions.Value;
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

    private async Task<string> StartTranscriptionAsync(NonDisposableStreamContent fileContent, string fileName, string language, CancellationToken cancellationToken)
    {
        using var content = new MultipartFormDataContent();
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);
        content.Add(new StringContent(language), "language");

        var response = await httpClient.PostAsync(TranscriptionsEndpoint, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    private async Task<WhisperProcessResult> GetTranscriptionAsync(string id, CancellationToken cancellationToken)
    {
        var endpoint = TranscriptionEndpoint.Replace("{id}", id);
        var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
        try
        {
            var result = JsonSerializer.Deserialize<WhisperProcessResult>(stringResult, _jsonOptions.SerializerOptions);
            if (result == null)
            {
                throw new Exception("Failed to deserialize transcription result.");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize transcription result: {Result}", stringResult);
            throw;
        }
    }

    private async IAsyncEnumerable<WhisperEvent?> ReadSseStreamAsync(HttpResponseMessage response, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            // SSE lines look like: "data: { ... }"
            if (line.StartsWith("data: "))
            {
                var json = line.Substring("data: ".Length);

                WhisperEvent? evt;
                try
                {
                    evt = JsonSerializer.Deserialize<WhisperEvent>(json, _jsonOptions.SerializerOptions);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to parse SSE JSON: {Json}", json);
                    continue;
                }

                yield return evt;
            }
        }
    }

    private async IAsyncEnumerable<WhisperEvent?> ReadSseStreamAsync(string id, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var endpoint = StreamTranscriptionEndpoint.Replace("{id}", id);
        var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        await foreach (var evt in ReadSseStreamAsync(response, cancellationToken))
        {
            yield return evt;
        }
    }

    public async Task<string> DetectLanguageAsync(NonDisposableStreamContent fileContent, string fileName, CancellationToken cancellationToken)
    {
        using var content = new MultipartFormDataContent();

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);

        var response = await httpClient.PostAsync("/detect-language", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        var language = await response.Content.ReadAsStringAsync(cancellationToken);
        return language;
    }

    public async Task<TranscriptionResponse> TranscribeAsync(Resource file, CancellationToken cancellationToken)
    {

        cancellationToken.Register(() => _logger.LogWarning("Cancellation token triggered"));
        var resourceContent = await _resourceService.GetResourceStreamByIdAsync(file.Id, cancellationToken);
        var nonDisposableContent = new NonDisposableStreamContent(resourceContent);

        var language = await DetectLanguageAsync(nonDisposableContent, file.FileName, cancellationToken);
        var id = await StartTranscriptionAsync(nonDisposableContent, file.FileName, language, cancellationToken);

        var eventStream = ReadSseStreamAsync(id, cancellationToken);

        await foreach (var output in eventStream.WithCancellation(cancellationToken))
        {
            if (output is WhisperEndEvent endEvent)
            {
                var result = await GetTranscriptionAsync(id, cancellationToken);
                if (result is WhisperProcessSuccessResult successResult)
                {
                    return new TranscriptionResponse()
                    {
                        ModelName = successResult.Data.Model.Type,
                        Provider = TranscriptionProvider.LocalWhisper,
                        Language = successResult.Data.Params.Language,
                        Items = successResult.Data.Transcription.Select(ti => new TranscriptionResponseItem()
                        {
                            Text = ti.Text,
                            TimeStamp = new TranscriptionResponseTimeStamp()
                            {
                                From = TimeSpan.FromMilliseconds(ti.Offsets.From),
                                To = TimeSpan.FromMilliseconds(ti.Offsets.To)
                            },
                            Confidence = ti.Tokens.Average(t => t.P)
                        })
                    };
                }
                else if (result is WhisperProcessErrorResult errorResult)
                {
                    throw new Exception($"Transcription failed: {errorResult.ErrorMessage}");
                }
                else
                {
                    throw new Exception("Unknown transcription result.");
                }
            }
            else if (output is WhisperLogEvent logEvent)
            {
                _logger.LogInformation("Transcription log: {Message}", logEvent.Message);
            }
        }

        throw new Exception("Transcription did not complete successfully.");
    }
}