using System.Diagnostics;
using System.Net.Http.Headers;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.MediaProcessing.Transcription;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class TranscriptionJobHandler : MediaJobHandlerBase<TranscriptionMediaProcessingJob>
{
    private static readonly List<string> SupportedFormats = new List<string> { "audio/mpeg", "audio/mp3", "audio/wav", "audio/x-wav", "audio/webm", "video/mp4", "video/mpeg", "audio/mp4", "audio/m4a", "audio/mpga" };
    public MediaJobType Type => MediaJobType.Transcription;
    private readonly ITranscriptionService _transcriptionService;
    public TranscriptionJobHandler(ApplicationDbContext db, ITranscriptionService transcriptionService) : base(db)
    {
        _transcriptionService = transcriptionService;
    }

    public async override Task HandleAsync(TranscriptionMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {
        var previousJobs = await ListAllPreviousJobsAsync(job, token);
        var resource = previousJobs.OfType<MediaTranscodingMediaProcessingJob>().Select(pj => pj.OutputResource).FirstOrDefault(p => p != null && SupportedFormats.Contains(p.MimeType));
        
        if (resource == null)
        {
            throw new Exception("No suitable converted media found for transcription.");
        }

        var transcriptionText = await _transcriptionService.TranscribeAsync(resource, token);
    }
}
