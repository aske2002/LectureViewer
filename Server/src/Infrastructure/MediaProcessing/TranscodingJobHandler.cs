using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.Models;
using backend.Infrastructure.Services;

public class TranscodingJobHandler : MediaJobHandlerBase<MediaTranscodingMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.MediaTranscoding;
    private readonly IResourceService _resourceService;
    private readonly IMediaService _mediaService;
    private readonly ApplicationDbContext _db;

    public TranscodingJobHandler(ApplicationDbContext db, IResourceService resourceService, IMediaService mediaService)
    {
        _resourceService = resourceService;
        _mediaService = mediaService;
        _db = db;
    }

    public async override Task HandleAsync(MediaTranscodingMediaProcessingJob job, MediaProcessingJobAttempt attempt, Resource? inputResource, CancellationToken token)
    {
        var resource = await job.GetMatchingResource(_db, (r) => MimeTypeHelpers.IsAudioMimeType(r.MimeType) ||
                                                                     MimeTypeHelpers.IsImageMimeType(r.MimeType) ||
                                                                     MimeTypeHelpers.IsVideoMimeType(r.MimeType), token);
        if (resource == null)
        {
            throw new Exception("Input resource not found.");
        }

        var resourceContent = await _resourceService.GetResourceContentByIdAsync(resource.Id, token);

        if (resourceContent == null)
        {
            throw new Exception("Input resource content not found.");
        }

        var fileStream = resourceContent.OpenReadStream();
        var transcodedData = await _mediaService.TranscodeMediaAsync(
                fileStream,
                resourceContent.FileName,
                new TranscodeRequest(job.TargetFormat, job.TargetBitrateKbps, job.TargetWidth, job.TargetHeight),
                token
            );

        var outputFileName = $"{Path.GetFileNameWithoutExtension(resourceContent.FileName)}.{job.TargetFormat}";
        var outputResource = await _resourceService.CreateResourceAsync(
            outputFileName,
            ResourceType.Media,
            transcodedData,
            order: null,
            cancellationToken: token
        );
        job.OutputResourceId = outputResource.Id;
        job.OutputResource = outputResource;
    }
}
