public static class MimeTypeHelpers
{
    private static readonly string [] imageMimeTypes = new[] {
                "image/jpeg",
                "image/png",
                "image/gif",
                "image/bmp",
                "image/webp",
                "image/tiff",
                "image/svg+xml"
            };
    
    private static readonly string[] documentMimeTypes = new[] {
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "application/vnd.ms-powerpoint",
                "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "application/pdf",
                "text/plain",
            };

    private static readonly string[] videoMimeTypes = new[] {
                "video/mp4",
                "video/avi",
                "video/mov",
                "video/mpeg",
                "video/quicktime"
            };

    private static readonly string[] audioMimeTypes = new[] {
                "audio/mpeg",
                "audio/wav",
                "audio/ogg",
                "audio/flac"
            };

    public static bool IsImageMimeType(string mimeType) => imageMimeTypes.Contains(mimeType);
    public static bool IsDocumentMimeType(string mimeType) => documentMimeTypes.Contains(mimeType);
    public static bool IsVideoMimeType(string mimeType) => videoMimeTypes.Contains(mimeType);
    public static bool IsAudioMimeType(string mimeType) => audioMimeTypes.Contains(mimeType);
}