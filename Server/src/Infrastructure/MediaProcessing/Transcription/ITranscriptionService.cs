using backend.Domain.Entities;

namespace backend.Infrastructure.MediaProcessing.Transcription;

public interface ITranscriptionService
{
    Task<TranscriptionResponse> TranscribeAsync(Resource file, CancellationToken cancellationToken);
}