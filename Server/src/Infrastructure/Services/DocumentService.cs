using System.Net.Http.Headers;
using System.Net.Http.Json;
using backend.Infrastructure.Models;

namespace backend.Infrastructure.Services;

public interface IDocumentService
{
    Task<byte[]> ConvertAsync(Stream fileStream, string fileName, ConvertDocumentRequest options, CancellationToken cancellationToken = default);
    Task<byte[]> ExtractDocumentThumbnailAsync(Stream fileStream, string fileName, ExtractDocumentThumbnailRequest request, CancellationToken cancellationToken);
    Task<DocumentDetailsResponse> GetDetailsAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);
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
    private static readonly string DetailsEndpoint = "/details";

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

    public async Task<DocumentDetailsResponse> GetDetailsAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken)
    {
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync(DetailsEndpoint, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Document details retrieval failed: {response.StatusCode} - {error}");
        }

        var responseContent = await response.Content.ReadFromJsonAsync<DocumentDetailsResponse>(cancellationToken: cancellationToken);

        if (responseContent == null)
        {
            throw new Exception("Failed to deserialize document details response.");
        }

        return responseContent;
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