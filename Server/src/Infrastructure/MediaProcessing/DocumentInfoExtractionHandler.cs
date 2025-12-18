using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.Models;
using backend.Infrastructure.Services;
using Microsoft.SemanticKernel.ChatCompletion;

public class DocumentInfoExtractionHandler : MediaJobHandlerBase<DocumentInfoExtractionJob>
{
    public MediaJobType Type => MediaJobType.DocumentInfoExtraction;
    private readonly IResourceService _resourceService;
    private readonly IMediaService _mediaService;
    private readonly IDocumentService _documentService;
    private readonly ISemanticService _semanticService;
    private readonly ApplicationDbContext _db;

    public DocumentInfoExtractionHandler(ApplicationDbContext db, IResourceService resourceService, IMediaService mediaService, IDocumentService documentService, ISemanticService semanticService)
    {
        _resourceService = resourceService;
        _mediaService = mediaService;
        _db = db;
        _documentService = documentService;
        _semanticService = semanticService;
    }

    public async override Task HandleAsync(DocumentInfoExtractionJob job, MediaProcessingJobAttempt attempt, Resource? inputResource, CancellationToken token)
    {
        if (inputResource == null)
        {
            throw new Exception("Input resource is null.");
        }

        var resourceContent = await _resourceService.GetResourceContentByIdAsync(inputResource.Id, token);
        if (resourceContent == null)
        {
            throw new Exception("Input resource content not found.");
        }

        var details = await _documentService.GetDetailsAsync(resourceContent.OpenReadStream(), resourceContent.FileName, token);

        var content = string.Join("\n", details.Pages.Select(p => p.Text));

        var prompt = new ChatHistory();
        prompt.AddSystemMessage("You are an AI assistant that extracts key information and summarizes documents. Respond with a detailed summary of the document content, in a factual declarative style. Return JSON in the given schema.");
        prompt.AddUserMessage("DOCUMENT CONTENT:\n\n" + content);

        var response = await _semanticService.GetChatCompletionAsync<DocumentSummaryExtractionResponse>(prompt, token);

        job.Document = new Document
        {
            ResourceId = inputResource.Id,
            Resource = inputResource,
            NumberOfPages = details.Pages.Count,
            Author = details.Author,
            Title = details.Title,
            Pages = details.Pages.Select(p => new DocumentPage
            {
                PageNumber = p.PageNumber,
                TextContent = p.Text,
            }).ToList(),
            Summary = response.Summary
        };
    }
}
