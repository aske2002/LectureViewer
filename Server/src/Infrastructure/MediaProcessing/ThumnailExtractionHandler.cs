using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.Models;
using backend.Infrastructure.Services;

public class ThumnailExtractionHandler : MediaJobHandlerBase<ThumbnailExtractionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.ThumbnailExtraction;
    private readonly IResourceService _resourceService;
    private readonly IMediaService _mediaService;
    private readonly IDocumentService _documentService;
    public ThumnailExtractionHandler(ApplicationDbContext db, IResourceService resourceService, IMediaService mediaService, IDocumentService documentService) : base(db)
    {
        _resourceService = resourceService;
        _mediaService = mediaService;
        _documentService = documentService;
    }

    public async override Task HandleAsync(ThumbnailExtractionMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {
        var allPreviousJobs = await ListAllPreviousJobsAsync(job, token);
        var resource = allPreviousJobs
            .Select(pj =>
            {
                if (pj is MediaTranscodingMediaProcessingJob mtj &&
                    mtj.OutputResource != null &&
                   (MimeTypeHelpers.IsVideoMimeType(mtj.OutputResource.MimeType) || MimeTypeHelpers.IsAudioMimeType(mtj.OutputResource.MimeType)))
                {
                    return mtj.OutputResource;
                }
                else if (pj is OfficeConversionMediaProcessingJob ocj &&
                    ocj.OutputResource != null &&
                    MimeTypeHelpers.IsDocumentMimeType(ocj.OutputResource.MimeType))
                {
                    return ocj.OutputResource;
                }
                return null;
            })
            .FirstOrDefault(r => r != null);

        if (resource == null)
        {
            throw new Exception("No suitable converted media or document found for thumbnail extraction.");
        }

        var resourceContent = await _resourceService.GetResourceContentByIdAsync(resource.Id, token);

        if (resourceContent == null)
        {
            throw new Exception("Input resource content not found.");
        }

        byte[]? thumnailData = null;
        if (MimeTypeHelpers.IsVideoMimeType(resource.MimeType))
        {
            thumnailData = await _mediaService.ExtractThumbnailAsync(
                resourceContent.OpenReadStream(),
                resourceContent.FileName,
                timePercentage: 10,
                width: job.Width,
                height: job.Height,
                cancellationToken: token
            );
        }
        else if (MimeTypeHelpers.IsDocumentMimeType(resource.MimeType))
        {
            var request = new ExtractDocumentThumbnailRequest
            {
                Width = job.Width,
                Height = job.Height,
                PageNumber = 1
            };

            thumnailData = await _documentService.ExtractDocumentThumbnailAsync(
                resourceContent.OpenReadStream(),
                resourceContent.FileName,
                request,
                token
            );
        }

        if (thumnailData != null)
        {
            var outputResource = await _resourceService.CreateResourceAsync(
                Path.GetFileNameWithoutExtension(resourceContent.FileName) + "_thumbnail.png",
                ResourceType.Thumbnail,
                thumnailData,
                order: null,
                cancellationToken: token
            );
            job.OutputResourceId = outputResource.Id;
            job.OutputResource = outputResource;
        }
    }
}
