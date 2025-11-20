namespace backend.Infrastructure.Models;
    
public record ExtractDocumentThumbnailRequest(int? Width = null, int? Height = null, int? PageNumber = null);