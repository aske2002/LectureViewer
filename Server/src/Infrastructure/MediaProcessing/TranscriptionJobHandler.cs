using backend.Domain.Entities;
using backend.Domain.Events;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.MediaProcessing.Transcription;
using backend.Infrastructure.Models;
using backend.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;

public class TranscriptionJobHandler : MediaJobHandlerBase<TranscriptionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.Transcription;
    private readonly ITranscriptionService _transcriptionService;
    private readonly ApplicationDbContext _db;
    private readonly ISemanticService _semanticService;
    private readonly ILogger<TranscriptionJobHandler> _logger;
    public TranscriptionJobHandler(ApplicationDbContext db, ITranscriptionService transcriptionService, ISemanticService semanticService, ILogger<TranscriptionJobHandler> logger) : base(db)
    {
        _transcriptionService = transcriptionService;
        _db = db;
        _semanticService = semanticService;
        _logger = logger;
    }

    private async Task<TranscriptionResponse> CorrectTranscriptionAsync(TranscriptionResponse transcript, CancellationToken token)
    {
        List<TranscriptionResponseItem> correctedItems = new List<TranscriptionResponseItem>();
        var chunks = transcript.Items.Chunk(50);

        foreach (var chunk in chunks)
        {
            var correctionPrompt = new ChatHistory();
            var chunkItems = string.Join("\n", chunk.Select((i, index) => $"{index}: {i.Text}").ToList());
            correctionPrompt.AddSystemMessage("You are a professional corrector. Correct grammar issues and missing/misused words in the following transcription items, using the same language as the source text, and return JSON in the given schema. Each item is formatted as 'index: text', in the response these need to match for EVERY item.");
            correctionPrompt.AddUserMessage(chunkItems);
            var correctionResponse = await _semanticService.GetChatCompletionAsync<List<CorrectedTranscriptionItemResponse>>(correctionPrompt, token);
            foreach (var correctedItem in correctionResponse)
            {
                if (correctedItem.Index < 0 || correctedItem.Index >= chunk.Length)
                {
                    _logger.LogWarning("Corrected item index {Index} is out of bounds for the original chunk length {Length}. Skipping item.", correctedItem.Index, chunk.Length);
                    continue;
                }

                var originalItem = chunk[correctedItem.Index];
                correctedItems.Add(new TranscriptionResponseItem
                {
                    Text = correctedItem.Text,
                    TimeStamp = originalItem.TimeStamp,
                    Confidence = originalItem.Confidence
                });
            }
            _logger.LogInformation("Corrected {Count} of {Total} transcription items in current chunk.", correctedItems.Count, transcript.Items.Count());
        }
        
        transcript.Items = correctedItems;
        return transcript;
    }

    public async override Task HandleAsync(TranscriptionMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {
        var resource = await FirstResourceOrDefaultAsync(job, (r) => MimeTypeHelpers.IsAudioMimeType(r.MimeType) || MimeTypeHelpers.IsVideoMimeType(r.MimeType), token);

        if (resource == null)
        {
            throw new Exception("No suitable converted media found for transcription.");
        }

        var transcriptionResponse = await _transcriptionService.TranscribeAsync(resource, token);
        var correctedTranscription = await CorrectTranscriptionAsync(transcriptionResponse, token);

        var transcript = new Transcript
        {
            SourceId = resource.Id,
            Source = resource,
            JobId = job.Id,
            Job = job,
            Items = correctedTranscription.Items.Select(i => new TranscriptItem
            {

                Text = i.Text,
                From = i.TimeStamp.From,
                To = i.TimeStamp.To,
                Confidence = i.Confidence

            }).ToList(),
            Language = correctedTranscription.Language,
            TranscriptText = correctedTranscription.FullText,
        };

        await _db.Transcripts.AddAsync(transcript, token);
        await _db.SaveChangesAsync(token);
    }
}
