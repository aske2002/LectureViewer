using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Infrastructure.Background;
using backend.Infrastructure.MediaProcessing.Configurations;
using backend.Infrastructure.MediaProcessing.Transcription;
using backend.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace backend.Infrastructure.MediaProcessing;

public static class DependencyInjection
{
    public static void AddMediaProcessing(this IHostApplicationBuilder builder)
    {

        // Background services
        builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        builder.Services.AddHostedService<QueuedHostedService>();
        builder.Services.AddHostedService<MediaJobWorker>();

        // Media processing services
        builder.Services.AddScoped<IMediaJobService, MediaJobService>();
        builder.Services.AddScoped<IMediaJobHandler<OfficeConversionMediaProcessingJob>, LibreOfficeConverterHandler>();
        builder.Services.AddScoped<IMediaJobHandler<MediaTranscodingMediaProcessingJob>, TranscodingJobHandler>();
        builder.Services.AddScoped<IMediaJobHandler<TranscriptionMediaProcessingJob>, TranscriptionJobHandler>();
        builder.Services.AddScoped<IMediaJobHandler<ThumbnailExtractionMediaProcessingJob>, ThumnailExtractionHandler>();

        var transcriptionConfig = builder.Configuration
            .GetSection("Transcription")
            .Get<TranscriptionConfiguration>();

        Guard.Against.Null(transcriptionConfig, message: "Transcription configuration is missing.");

        if (!string.IsNullOrEmpty(transcriptionConfig.LocalTranscriptionApiHost))
        {
            builder.Services.AddHttpClient<ITranscriptionService, LocalTranscriptionService>(c => {
                c.BaseAddress = new Uri(transcriptionConfig.LocalTranscriptionApiHost);
            });
        }
        else
        {
            throw new InvalidOperationException(
                "No valid transcription configuration provided.");
        }
    }
}
