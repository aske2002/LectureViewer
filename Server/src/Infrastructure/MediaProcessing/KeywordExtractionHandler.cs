using backend.Domain.Entities;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.Models;
using backend.Infrastructure.Services;
using Microsoft.SemanticKernel.ChatCompletion;

public class KeywordExtractionHandler : MediaJobHandlerBase<KeywordExtractionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.KeywordExtraction;
    private readonly ISemanticService _semanticService;
    private readonly IKeywordService _keywordService;

    public KeywordExtractionHandler(ISemanticService semanticService, IKeywordService keywordService)
    {
        _semanticService = semanticService;
        _keywordService = keywordService;
    }

    private float Cosine(ReadOnlySpan<float> a, ReadOnlySpan<float> b)
    {
        float dot = 0, normA = 0, normB = 0;

        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            normA += a[i] * a[i];
            normB += b[i] * b[i];
        }

        return dot / (MathF.Sqrt(normA) * MathF.Sqrt(normB));
    }

    public async override Task HandleAsync(KeywordExtractionMediaProcessingJob job, MediaProcessingJobAttempt attempt, Resource? inputResource, CancellationToken token)
    {
        var prompt = new ChatHistory();
        prompt.AddSystemMessage(@"You are a keyword extraction engine.
You extract about 20-30 of the most important, specific, and domain-relevant keywords from the provided text.
Always follow the user’s instructions exactly.
Return clean JSON in the given schema, with no extra text or commentary..");

        var keywordsResponses = await _semanticService.GetLongTextCompletionAsync<KeywordExtractionResponse>(
            prompt,
            job.SourceText,
            chunkFormatter: (chunk) => @$"Extract the most important keywords from the given text.
Rules:
- Focus on specific and domain-relevant terms.
- Avoid generic or overly broad terms.
- Only include terms that occur in the text one to one.

THE TEXT:
{chunk}",
            maxTokens: 128000,
            cancellationToken: token);

        var keywords = keywordsResponses
            .SelectMany(kr => kr.Keywords)
            .Distinct()
            .Where(k => job.SourceText.Contains(k, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var savedKeywords = await _keywordService.CreateOrGetKeywordsAsync(
            keywords,
            token);

        job.ExtractedKeywords = savedKeywords.ToList();
    }
}