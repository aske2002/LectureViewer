using System.Diagnostics;
using System.Net.Http.Headers;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.MediaProcessing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class LibreOfficeConverterHandler : MediaJobHandlerBase<OfficeConversionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.OfficeConversion;
    private readonly ILogger<LibreOfficeConverterHandler> _logger;
    private readonly IResourceService _resourceService;
    private readonly IApplicationDbContext _db;

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;


    public LibreOfficeConverterHandler(ILogger<LibreOfficeConverterHandler> logger, IResourceService resourceService, IApplicationDbContext db, IOptions<LibreOfficeSettings> settings,
        HttpClient httpClient)
    {
        _baseUrl = settings.Value.BaseUrl.TrimEnd('/');
        _httpClient = httpClient;
        _logger = logger;
        _resourceService = resourceService;
        _db = db;
    }

    private async Task<byte[]> ConvertAsync(
                Stream fileStream,
        string fileName,
        string outputFormat = "pdf",
        string? filterOptions = null,
        string? outputFilename = null)
    {
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        content.Add(fileContent, "file", fileName);
        content.Add(new StringContent(outputFormat), "output_format");

        if (filterOptions != null)
            content.Add(new StringContent(filterOptions), "filter_options");

        if (outputFilename != null)
            content.Add(new StringContent(outputFilename), "output_filename");

        var response = await _httpClient.PostAsync($"{_baseUrl}/convert", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"LibreOffice conversion failed: {response.StatusCode} - {error}");
        }

        return await response.Content.ReadAsByteArrayAsync();
    }

    public async override Task HandleAsync(OfficeConversionMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {

        var resourceContent = await _resourceService.GetResourceContentByIdAsync(job.InputResourceId, token);
        var fileStream = resourceContent.OpenReadStream();

        var outputFile = await ConvertAsync(fileStream, job.TargetFormat);
        var fileName = Path.GetFileNameWithoutExtension(resourceContent.FileName) + "." + job.TargetFormat;

        var resource = await _resourceService.AddAssociatedResourceAsync(job.InputResourceId, fileName, ResourceType.Document, outputFile, cancellationToken: token);

        job.OutputResourceId = resource.Id;
        job.OutputResource = resource;

        await _db.SaveChangesAsync(token);
        _logger.LogInformation("Conversion complete: {Output}", resource.Id);
    }
}
