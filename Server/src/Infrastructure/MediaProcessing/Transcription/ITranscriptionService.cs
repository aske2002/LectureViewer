using backend.Domain.Entities;

namespace backend.Infrastructure.MediaProcessing.Transcription;

public interface ITranscriptionService
{
    Task<string> TranscribeAsync(Resource file, CancellationToken cancellationToken);
}