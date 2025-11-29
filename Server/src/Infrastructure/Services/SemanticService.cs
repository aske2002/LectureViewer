using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OllamaSharp;
using OpenAI;

namespace backend.Infrastructure.Services;

public interface ISemanticService
{
    Task<T> GetChatCompletionAsync<T>(ChatHistory messages, CancellationToken cancellationToken = default);
}

public class SemanticService : ISemanticService
{
    private readonly IChatCompletionService _chatCompletionService;
    private readonly JsonOptions _jsonOptions;
    private readonly IServiceProvider _serviceProvider;

    public SemanticService(IChatCompletionService chatCompletionService, IOptions<JsonOptions> jsonOptions, IServiceProvider serviceProvider)
    {
        _chatCompletionService = chatCompletionService;
        _jsonOptions = jsonOptions.Value;
        _serviceProvider = serviceProvider;
    }

    public async Task<T> GetChatCompletionAsync<T>(ChatHistory messages, CancellationToken cancellationToken = default)
    {
        ChatOptions chatOptions = new ChatOptions()
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<T>()
        };

        var settings = new OpenAIPromptExecutionSettings()
        {
            ResponseFormat = typeof(T)
        };


        ChatResponse response = await _chatCompletionService.AsChatClient().GetResponseAsync(messages.Select(m => m.ToChatMessage()), chatOptions, cancellationToken);
        var content = response.Text;
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