using System.Text.Json;
using backend.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using OllamaSharp.Models;
using OpenAI;

namespace backend.Infrastructure.Services;

public interface ISemanticService
{
    Task<T> GetChatCompletionAsync<T>(ChatHistory messages, CancellationToken cancellationToken = default);
    Task<GeneratedEmbeddings<Embedding<float>>> GenerateEmbeddingsAsync(List<string> input, CancellationToken cancellationToken = default);
    Task<GeneratedEmbeddings<Embedding<float>>> GenerateFastEmbeddingsAsync(List<string> input, CancellationToken cancellationToken = default);
    Task<Embedding<float>> GenerateEmbeddingAsync(string input, CancellationToken cancellationToken = default);
    Task<Embedding<float>> GenerateFastEmbeddingAsync(string input, CancellationToken cancellationToken = default);
}

public class SemanticService : ISemanticService
{
    private bool _modelChecked = false;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingService;
    private readonly JsonOptions _jsonOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly MLConfiguration _mlConfig;
    private readonly ILogger<SemanticService> _logger;

    private string SmallEmbeddingModelName => _mlConfig.SmallEmbeddingModelName ?? _mlConfig.EmbeddingModelName ?? _mlConfig.ModelName;
    private string EmbeddingModelName => _mlConfig.EmbeddingModelName ?? _mlConfig.ModelName;
    private string ModelName => _mlConfig.ModelName;

    public SemanticService(IChatCompletionService chatCompletionService, IEmbeddingGenerator<string, Embedding<float>> embeddingService, IOptions<JsonOptions> jsonOptions, IServiceProvider serviceProvider, IOptions<MLConfiguration> mlConfig, ILogger<SemanticService> logger)
    {
        _chatCompletionService = chatCompletionService;
        _embeddingService = embeddingService;
        _jsonOptions = jsonOptions.Value;
        _serviceProvider = serviceProvider;
        _mlConfig = mlConfig.Value;
        _logger = logger;
    }

    private async Task EnsureModelIsAvailableAsync(CancellationToken cancellationToken)
    {
        var ollamaChatClient = _serviceProvider.GetServices<OllamaApiClient>().FirstOrDefault();

        if (_modelChecked || ollamaChatClient == null)
        {
            return;
        }

        var models = await ollamaChatClient.ListLocalModelsAsync(cancellationToken);
        var requestdModels = new List<string> { ModelName, EmbeddingModelName, SmallEmbeddingModelName };
        foreach (var requestedModel in requestdModels)
        {
            if (!models.Any(m => m.Name == requestedModel))
            {
                IAsyncEnumerable<PullModelResponse?> response = ollamaChatClient.PullModelAsync(requestedModel, cancellationToken);

                await foreach (var item in response.WithCancellation(cancellationToken))
                {
                    _logger.LogInformation("Pulling model {ModelName}: {Progress}%", requestedModel, item?.Percent);

                    if (item?.Status == "success")
                    {
                        _logger.LogInformation("Model {ModelName} pulled successfully.", requestedModel);
                        break;
                    }
                }
            }
        }

        _modelChecked = true;
    }

    public async Task<Embedding<float>> GenerateFastEmbeddingAsync(string input, CancellationToken cancellationToken = default)
    {
        await EnsureModelIsAvailableAsync(cancellationToken);
        var embeddingResponse = await _embeddingService.GenerateAsync(input, new EmbeddingGenerationOptions
        {
            ModelId = SmallEmbeddingModelName
        }, cancellationToken: cancellationToken);
        return embeddingResponse;
    }

    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateFastEmbeddingsAsync(List<string> input, CancellationToken cancellationToken = default)
    {
        await EnsureModelIsAvailableAsync(cancellationToken);
        var embeddingResponse = await _embeddingService.GenerateAsync(input, new EmbeddingGenerationOptions
        {
            ModelId = SmallEmbeddingModelName
        }, cancellationToken: cancellationToken);
        return embeddingResponse;
    }

    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateEmbeddingsAsync(List<string> input, CancellationToken cancellationToken = default)
    {

        await EnsureModelIsAvailableAsync(cancellationToken);
        var embeddingResponse = await _embeddingService.GenerateAsync(input, new EmbeddingGenerationOptions
        {
            ModelId = EmbeddingModelName
        }, cancellationToken: cancellationToken);
        return embeddingResponse;
    }

    public async Task<Embedding<float>> GenerateEmbeddingAsync(string input, CancellationToken cancellationToken = default)
    {
        await EnsureModelIsAvailableAsync(cancellationToken);
        var embeddingResponse = await _embeddingService.GenerateAsync(input, new EmbeddingGenerationOptions
        {
            ModelId = EmbeddingModelName
        }, cancellationToken: cancellationToken);
        return embeddingResponse;
    }

    public async Task<T> GetChatCompletionAsync<T>(ChatHistory messages, CancellationToken cancellationToken = default)
    {
        await EnsureModelIsAvailableAsync(cancellationToken);

        ChatOptions chatOptions = new ChatOptions()
        {
            // AdditionalProperties = new AdditionalPropertiesDictionary
            // {
            //     { "think", true }
            // },
            ModelId = ModelName,
            ResponseFormat = ChatResponseFormat.ForJsonSchema<T>()
        };


        IAsyncEnumerable<ChatResponseUpdate> streamResponse = _chatCompletionService.AsChatClient().GetStreamingResponseAsync(messages.Select(m => m.ToChatMessage()), chatOptions, cancellationToken);
        IEnumerable<ChatResponseUpdate> updates = Array.Empty<ChatResponseUpdate>();

        await foreach (var update in streamResponse.WithCancellation(cancellationToken))
        {
            updates = updates.Append(update);
            if (update.FinishReason is not null)
            {
                break;
            }
        }

        var finalResponse = updates.ToChatResponse();

        var content = finalResponse.Text;
        if (content is null)
        {
            throw new Exception("No response from OpenAI chat completion.");
        }

        var deserializedContent = JsonSerializer.Deserialize<T>(content, _jsonOptions.SerializerOptions);
        if (deserializedContent is null)
        {
            throw new Exception("Failed to deserialize OpenAI chat completion response.");
        }
        return deserializedContent;

    }
}