namespace backend.Infrastructure.Models;
    
public record ConvertDocumentRequest(string format = "pdf", string[]? FilterOptions = null);