using backend.Domain.Entities;

namespace backend.Infrastructure.MediaProcessing.Transcription;

public class LocalTranscriptionService : ITranscriptionService
{
    public async Task<string> TranscribeAsync(Resource file, CancellationToken cancellationToken)
    {
        // Implement local transcription logic here
        return await Task.FromResult("Transcription result");
    }
}