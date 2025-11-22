using backend.Domain.Entities;
using backend.Domain.Events;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.MediaProcessing.Transcription;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class TranscriptionJobHandler : MediaJobHandlerBase<TranscriptionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.Transcription;
    private readonly ITranscriptionService _transcriptionService;
    private readonly ApplicationDbContext _db;
    private readonly IMediator _mediator;
    public TranscriptionJobHandler(ApplicationDbContext db, ITranscriptionService transcriptionService, IMediator mediator) : base(db)
    {
        _transcriptionService = transcriptionService;
        _db = db;
        _mediator = mediator;
    }

    public async override Task HandleAsync(TranscriptionMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {
        var resource = await FirstResourceOrDefaultAsync(job, (r) => MimeTypeHelpers.IsAudioMimeType(r.MimeType) || MimeTypeHelpers.IsVideoMimeType(r.MimeType), token);

        if (resource == null)
        {
            throw new Exception("No suitable converted media found for transcription.");
        }

        var transcriptionResponse = await _transcriptionService.TranscribeAsync(resource, token);

        var transcript = new Transcript
        {
            SourceId = resource.Id,
            Source = resource,
            JobId = job.Id,
            Job = job,
            Items = transcriptionResponse.Items.Select(i => new TranscriptItem
            {
                
                Text = i.Text,
                From = i.TimeStamp.From,
                To = i.TimeStamp.To,
                Confidence = i.Confidence
                
            }).ToList(),
            Language = transcriptionResponse.Language,
            TranscriptText = transcriptionResponse.FullText,  
        };

        await _db.Transcripts.AddAsync(transcript, token);
        await _db.SaveChangesAsync(token);
        await _mediator.Publish(new TranscriptionCompletedEvent(transcript), token);
    }
}
