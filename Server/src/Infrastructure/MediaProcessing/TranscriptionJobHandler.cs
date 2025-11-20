using backend.Domain.Entities;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.MediaProcessing.Transcription;

public class TranscriptionJobHandler : MediaJobHandlerBase<TranscriptionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.Transcription;
    private readonly ITranscriptionService _transcriptionService;
    public TranscriptionJobHandler(ApplicationDbContext db, ITranscriptionService transcriptionService) : base(db)
    {
        _transcriptionService = transcriptionService;
    }

    public async override Task HandleAsync(TranscriptionMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {
        var resource = await FirstResourceOrDefaultAsync(job, (r) => MimeTypeHelpers.IsAudioMimeType(r.MimeType) || MimeTypeHelpers.IsVideoMimeType(r.MimeType), token);

        if (resource == null)
        {
            throw new Exception("No suitable converted media found for transcription.");
        }

        var transcriptionResponse = await _transcriptionService.TranscribeAsync(resource, token);

        
    }
}
