using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.Models;
using backend.Infrastructure.Services;
using Microsoft.SemanticKernel.ChatCompletion;

public class KeywordExtractionHandler : MediaJobHandlerBase<KeywordExtractionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.KeywordExtraction;
    private readonly ISemanticService _semanticService;
    public KeywordExtractionHandler(ApplicationDbContext db, ISemanticService semanticService) : base(db)
    {
        _semanticService = semanticService;
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

    private static float CosSim(float[] a, float[] b)
    {
        float dot = 0, na = 0, nb = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            na += a[i] * a[i];
            nb += b[i] * b[i];
        }
        return dot / (MathF.Sqrt(na) * MathF.Sqrt(nb));
    }

    public async override Task HandleAsync(KeywordExtractionMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {
        var candidates = KeywordCandidateExtractor.ExtractCandidatePhrases(job.SourceText, maxCandidates: 2000);

        var sentencesResponse = await _semanticService.GenerateEmbeddingsAsync(candidates, token);
        var embeddedCandidates = candidates.Zip(sentencesResponse, (Phrase, Vector) => (Phrase, Vector.Vector))
            .ToList();
        var fullTextEmbedding = await _semanticService.GenerateEmbeddingAsync(job.SourceText, token);

        var results = new List<(string Phrase, float Score)>();

        for (int i = 0; i < candidates.Count; i++)
        {
            var similarity = Cosine(
                fullTextEmbedding.Vector.Span,
                embeddedCandidates[i].Vector.Span
            );

            results.Add((candidates[i], similarity));
        }

        var rankedCandidates = results
            .OrderByDescending(r => r.Score)
            .Take(150) // top 50 → refine w/ LLM
            .ToList();

        var prompt = new ChatHistory();
        prompt.AddSystemMessage(@"You are a keyword extraction engine.
From the CANDIDATE KEYWORDS below, choose the 15–25 MOST important,
specific, and domain-relevant keywords or key phrases. Return them as JSON in the given schema.");
        prompt.AddUserMessage("CANDIDATE KEYWORDS:\n\n" + string.Join("\n", rankedCandidates));

        var response = await _semanticService.GetChatCompletionAsync<KeywordExtractionResponse>(prompt, token);
    }
}