using System.Net.Http.Headers;
using backend.Infrastructure.Models;

namespace backend.Infrastructure.Services;

public interface IDocumentService
{
    Task<byte[]> ConvertAsync(Stream fileStream, string fileName, ConvertDocumentRequest options, CancellationToken cancellationToken = default);
    Task<byte[]> ExtractDocumentThumbnailAsync(Stream fileStream, string fileName, ExtractDocumentThumbnailRequest request, CancellationToken cancellationToken);
}

public class DocumentService : IDocumentService
{
    private readonly HttpClient _httpClient;
    public DocumentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private static readonly string ConvertEndpoint = "/convert";
    private static readonly string ExtractThumbnailEndpoint = "/thumbnail";

    public async Task<byte[]> ExtractDocumentThumbnailAsync(Stream fileStream, string fileName, ExtractDocumentThumbnailRequest request, CancellationToken cancellationToken)
    {
        using var content = request.ToMultipart();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync(ExtractThumbnailEndpoint, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Thumbnail extraction failed: {response.StatusCode} - {error}");
        }

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    public async Task<byte[]> ConvertAsync(
        Stream fileStream,
       string fileName,
       ConvertDocumentRequest options,
       CancellationToken cancellationToken = default)
    {
        using var content = options.ToMultipart();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync(ConvertEndpoint, content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"LibreOffice conversion failed: {response.StatusCode} - {error}");
        }

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}