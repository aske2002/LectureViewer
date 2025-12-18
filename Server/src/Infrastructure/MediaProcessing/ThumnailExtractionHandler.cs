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
    private readonly ApplicationDbContext _db;

    public ThumnailExtractionHandler(ApplicationDbContext db, IResourceService resourceService, IMediaService mediaService, IDocumentService documentService)
    {
        _resourceService = resourceService;
        _mediaService = mediaService;
        _db = db;
        _documentService = documentService;
    }

    public async override Task HandleAsync(ThumbnailExtractionMediaProcessingJob job, MediaProcessingJobAttempt attempt, Resource? inputResource, CancellationToken token)
    {
        var resource = await job.GetMatchingResource(_db, (r) => MimeTypeHelpers.IsVideoMimeType(r.MimeType) ||
                                                                    MimeTypeHelpers.IsAudioMimeType(r.MimeType) ||
                                                                    MimeTypeHelpers.IsDocumentMimeType(r.MimeType), token);
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
            var outputResource = await _resourceService.AddAssociatedResourceAsync(resource.Id,
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
