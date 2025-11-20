using System.Diagnostics;
using System.Net.Http.Headers;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.Data;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.Models;
using backend.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class LibreOfficeConverterHandler : MediaJobHandlerBase<OfficeConversionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.OfficeConversion;
    private readonly ILogger<LibreOfficeConverterHandler> _logger;
    private readonly IResourceService _resourceService;
    private readonly ApplicationDbContext _db;

    private readonly IDocumentService _documentService;


    public LibreOfficeConverterHandler(ILogger<LibreOfficeConverterHandler> logger, IResourceService resourceService, ApplicationDbContext db,
        IDocumentService documentService) : base(db)
    {
        _documentService = documentService;
        _logger = logger;
        _resourceService = resourceService;
        _db = db;
    }
    public async override Task HandleAsync(OfficeConversionMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {

        var resource = await FirstResourceOrDefaultAsync(job, r => MimeTypeHelpers.IsDocumentMimeType(r.MimeType), token);


        if (resource == null)
        {
            throw new Exception("Input resource not found.");
        }

        var resourceContent = await _resourceService.GetResourceContentByIdAsync(resource.Id, token);
        var fileStream = resourceContent.OpenReadStream();

        var outputFile = await _documentService.ConvertAsync(fileStream, resourceContent.FileName, new ConvertDocumentRequest(job.TargetFormat));
        var fileName = Path.GetFileNameWithoutExtension(resourceContent.FileName) + "." + job.TargetFormat;

        var outputResource = await _resourceService.AddAssociatedResourceAsync(resource.Id, fileName, ResourceType.Document, outputFile, cancellationToken: token);

        job.OutputResourceId = outputResource.Id;
        job.OutputResource = outputResource;

        await _db.SaveChangesAsync(token);
        _logger.LogInformation("Conversion complete: {Output}", outputResource.Id);
    }
}
