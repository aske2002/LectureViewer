using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.AI;
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

    public SemanticService(IChatCompletionService chatCompletionService, IOptions<JsonOptions> jsonOptions)
    {
        _chatCompletionService = chatCompletionService;
        _jsonOptions = jsonOptions.Value;
    }

    public async Task<T> GetChatCompletionAsync<T>(ChatHistory messages, CancellationToken cancellationToken = default)
    {

        var chatClient = _chatCompletionService.AsChatClient().GetService<IChatClient>();
    

        if (chatClient is OllamaApiClient ollamaChatClient)
        {
            if (ollamaChatClient is null)
            {
                throw new Exception("Ollama chat client is not initialized.");
            }

            ChatResponse<T> response = await ollamaChatClient.GetResponseAsync<T>(messages.Select(m => m.ToChatMessage()), cancellationToken: cancellationToken);
            return response.Result;
        }
        else if (_chatCompletionService is OpenAIChatCompletionService)
        {
            var settings = new OpenAIPromptExecutionSettings()
            {
                ResponseFormat = typeof(T)
            };
            var response = await _chatCompletionService.GetChatMessageContentsAsync(messages, executionSettings: settings, cancellationToken: cancellationToken);
            var content = response.FirstOrDefault()?.Content;
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
        else
        {
            throw new NotSupportedException("Chat completion service is not supported for this request.");
        }

    }
}