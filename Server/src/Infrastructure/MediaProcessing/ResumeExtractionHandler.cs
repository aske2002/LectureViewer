using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.Models;
using backend.Infrastructure.Services;
using Microsoft.SemanticKernel.ChatCompletion;

public class ResumeExtractionHandler : MediaJobHandlerBase<ResumeExtractionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.ResumeExtraction;
    private readonly ISemanticService _semanticService;
    public ResumeExtractionHandler(ISemanticService semanticService)
    {
        _semanticService = semanticService;
    }

    public async override Task HandleAsync(ResumeExtractionMediaProcessingJob job, MediaProcessingJobAttempt attempt, Resource? inputResource, CancellationToken token)
    {
        var prompt = new ChatHistory();
        prompt.AddUserMessage("Generate a summary and return JSON in the given schema from the following text, using the same language as the source text:\n\n" + job.SourceText);

        var response = await _semanticService.GetChatCompletionAsync<ResumeExtractionResponse>(prompt, token);

        job.Resume = response.ResumeText;
    }
}
