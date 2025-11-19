using System.Net.Http.Headers;
using backend.Infrastructure.Models;

namespace backend.Infrastructure.Services;

public interface IMediaService
{
    Task<byte[]> ExtractThumbnailAsync(Stream fileStream, string fileName, TimeSpan timeSeconds, int? width = null, int? height = null, CancellationToken cancellationToken = default);
    Task<byte[]> ExtractThumbnailAsync(Stream fileStream, string fileName, int timePercentage, int? width = null, int? height = null, CancellationToken cancellationToken = default);
    Task<byte[]> TranscodeMediaAsync(Stream fileStream, string fileName, TranscodeRequest options, CancellationToken cancellationToken = default);
}

public class MediaService : IMediaService
{
    private readonly HttpClient _httpClient;
    public MediaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    private static readonly string TranscodeEndpoint = "transcode";
    private static readonly string ThumbnailEndpoint = "thumbnail";

    private async Task<byte[]> ExtractThumnailAsync(Stream fileStream, string fileName, ExtractThumbnailRequest request, CancellationToken cancellationToken)
    {
        using var content = request.ToMultipart();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync(ThumbnailEndpoint, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Thumbnail extraction failed: {response.StatusCode} - {error}");
        }

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
    public async Task<byte[]> ExtractThumbnailAsync(Stream fileStream, string fileName, TimeSpan timeSeconds, int? width = null, int? height = null, CancellationToken cancellationToken = default)
    {
        var request = new ExtractThumbnailRequest(TimeSeconds: timeSeconds.TotalSeconds, Width: width, Height: height);
        return await ExtractThumnailAsync(fileStream, fileName, request, cancellationToken);
    }

    public async Task<byte[]> ExtractThumbnailAsync(Stream fileStream, string fileName, int timePercentage, int? width = null, int? height = null, CancellationToken cancellationToken = default)
    {
        if (timePercentage < 0 || timePercentage > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(timePercentage), "Time percentage must be between 0 and 100.");
        }

        var request = new ExtractThumbnailRequest(TimePercentage: timePercentage, Width: width, Height: height);
        return await ExtractThumnailAsync(fileStream, fileName, request, cancellationToken);
    }


    public async Task<byte[]> TranscodeMediaAsync(Stream fileStream, string fileName, TranscodeRequest options, CancellationToken cancellationToken = default)
    {
        using var content = options.ToMultipart();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync(TranscodeEndpoint, content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Media transcoding failed: {response.StatusCode} - {error}");
        }

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}