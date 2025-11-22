using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.Models;
using backend.Infrastructure.Services;

public class LectureProcessingJobHandler : MediaJobHandlerBase<LectureProcessingJob>
{
    public MediaJobType Type => MediaJobType.LectureProcessing;
    private readonly ApplicationDbContext _db;
    private readonly IMediaJobService _mediaJobService;

    public LectureProcessingJobHandler(ApplicationDbContext db, IMediaJobService mediaJobService) : base(db)
    {
        _db = db;
        _mediaJobService = mediaJobService;
        _mediaJobService = mediaJobService;
    }

    public async override Task HandleAsync(LectureProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {
        await _db.LectureProcessingJobs.Entry(job).Reference(j => j.LectureContent).LoadAsync(token);
        await _db.LectureContents.Entry(job.LectureContent).Reference(lc => lc.Resource).LoadAsync(token);

        var resource = job.LectureContent.Resource;
        if (resource == null)
        {
            throw new Exception("No resource found for lecture content.");
        }

        else if (MimeTypeHelpers.IsDocumentMimeType(resource.MimeType))
        {
            // Create document processing job
            var officeConversionJob = new OfficeConversionMediaProcessingJob
            {
                InputResourceId = resource.Id,
                InputResource = resource,
                TargetFormat = "pdf",
                JobType = MediaJobType.OfficeConversion,
            };

            var thumbnailJob = new ThumbnailExtractionMediaProcessingJob
            {
                InputResourceId = resource.Id,
                InputResource = resource,
                JobType = MediaJobType.ThumbnailExtraction
            };

            await _mediaJobService.CreateJob(officeConversionJob, token);
            await _mediaJobService.CreateJob(thumbnailJob, token);
        }
        else if (MimeTypeHelpers.IsVideoMimeType(resource.MimeType) ||
                 MimeTypeHelpers.IsAudioMimeType(resource.MimeType))
        {
            var conversionJob = new MediaTranscodingMediaProcessingJob
            {
                ParentJob = job,
                InputResourceId = resource.Id,
                InputResource = resource,
                TargetFormat = "mp3",
                JobType = MediaJobType.MediaTranscoding,
            };

            var thumbnailJob = new ThumbnailExtractionMediaProcessingJob
            {
                ParentJob = job,
                InputResourceId = resource.Id,
                InputResource = resource,
                JobType = MediaJobType.ThumbnailExtraction,
            };

            var transcriptionJob = new TranscriptionMediaProcessingJob
            {
                
                ParentJob = conversionJob,
                JobType = MediaJobType.Transcription,
            };

            var categoryJob = new CategoryClassificationMediaProcessingJob
            {
                ParentJob = transcriptionJob,
                JobType = MediaJobType.CategoryClassification,
            };

            var keywordJob = new KeywordExtractionMediaProcessingJob
            {
                ParentJob = transcriptionJob,
                JobType = MediaJobType.KeywordExtraction,
            };

            var flashcardJob = new FlashcardGenerationMediaProcessingJob
            {
                ParentJob = keywordJob,
                JobType = MediaJobType.FlashcardGeneration,
            };

            await _mediaJobService.CreateJob(conversionJob, token);
            await _mediaJobService.CreateJob(thumbnailJob, token);
            await _mediaJobService.CreateJob(transcriptionJob, token);
            await _mediaJobService.CreateJob(categoryJob, token);
            await _mediaJobService.CreateJob(keywordJob, token);
            await _mediaJobService.CreateJob(flashcardJob, token);
        }
    }
}
