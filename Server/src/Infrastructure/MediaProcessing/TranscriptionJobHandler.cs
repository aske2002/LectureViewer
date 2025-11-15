using System.Diagnostics;
using System.Net.Http.Headers;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Infrastructure.MediaProcessing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class TranscriptionJobHandler : MediaJobHandlerBase<TranscriptionMediaProcessingJob>
{
    public MediaJobType Type => MediaJobType.Transcription;
    private readonly ILogger<LibreOfficeConverterHandler> _logger;
    private readonly IResourceService _resourceService;
    private readonly IApplicationDbContext _db;

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;


    public TranscriptionJobHandler(ILogger<LibreOfficeConverterHandler> logger, IResourceService resourceService, IApplicationDbContext db, IOptions<LibreOfficeSettings> settings,
        HttpClient httpClient)
    {
        _baseUrl = settings.Value.BaseUrl.TrimEnd('/');
        _httpClient = httpClient;
        _logger = logger;
        _resourceService = resourceService;
        _db = db;
    }

    public async override Task HandleAsync(TranscriptionMediaProcessingJob job, MediaProcessingJobAttempt attempt, CancellationToken token)
    {
        var previousJobs = await ListAllPreviousJobsAsync(job, token);
        var conversionJob = previousJobs.OfType<MediaConversionMediaProcessingJob>().FirstOrDefault();

        if (conversionJob is null)
        {
            throw new InvalidOperationException("No conversion job found for transcription job.");
        }

        if (conversionJob.OutputResourceId is null)
        {
            throw new InvalidOperationException("Conversion job has no output resource.");
        }
    }
}
