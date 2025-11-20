namespace backend.Infrastructure.Models;
    
public record ExtractThumbnailRequest(double? TimeSeconds = null, double? TimePercentage = null, int? Width = null, int? Height = null);