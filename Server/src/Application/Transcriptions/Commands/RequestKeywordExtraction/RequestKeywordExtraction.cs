using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Application.Transcriptions.Commands.RequestKeywordExtraction;

public record RequestKeywordExtractionCommand(TranscriptId TranscriptId) : IRequest;


public class RequestKeywordExtractionCommandHandler : IRequestHandler<RequestKeywordExtractionCommand>
{
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly IRepository<Transcript, TranscriptId> _transcriptRepository;


    public RequestKeywordExtractionCommandHandler(IBackgroundTaskQueue backgroundTaskQueue, IRepository<Transcript, TranscriptId> transcriptRepository)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _transcriptRepository = transcriptRepository;
    }

    public async Task Handle(RequestKeywordExtractionCommand request, CancellationToken cancellationToken)
    {
        var transcript = await _transcriptRepository.FindAsync(
            filter: t => t.Where(t => t.Id == request.TranscriptId),
            include: t => t.Include(t => t.Items)
        );

        if (transcript == null)
        {
            return;
        }

        _backgroundTaskQueue.QueueBackgroundWorkItem(async (sp, token) =>
        {
            var mediaJobService = sp.GetRequiredService<IMediaJobService>();

            await mediaJobService.CreateJob(new KeywordExtractionMediaProcessingJob()
            {
                SourceText = string.Join("\n", transcript.Items.Select(i => i.Text)),
            }, cancellationToken);
        });

    }
}
