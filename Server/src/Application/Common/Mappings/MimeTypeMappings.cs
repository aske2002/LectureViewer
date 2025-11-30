using backend.Domain.Enums;

public static class MimeTypeMappings
{
    private static readonly Dictionary<List<string>, LectureContentType> MimeTypeMap = new()
    {
        { new List<string> { "video/mp4", "video/mpeg", "video/quicktime", "video/x-msvideo", "video/x-ms-wmv" }, LectureContentType.Video },
        { new List<string> { "audio/mpeg", "audio/wav", "audio/ogg", "audio/aac" }, LectureContentType.Audio },
        { new List<string> { "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation", "application/pdf" }, LectureContentType.Presentation },
        { new List<string> { "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "text/plain", "application/vnd.oasis.opendocument.text" }, LectureContentType.Document },
    };

    public static LectureContentType GetLectureContentType(string mimeType)
    {
        foreach (var entry in MimeTypeMap)
        {
            if (entry.Key.Contains(mimeType))
            {
                return entry.Value;
            }
        }

        return LectureContentType.Other;
    }
}