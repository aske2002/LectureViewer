using System.Text.Json.Serialization;

public static class MimeTypeHelpers
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Flags]
    public enum MimeTypes
    {
        Jpeg,
        Png,
        Gif,
        Bmp,
        Webp,
        Tiff,
        Svg,
        Word,
        WordOpenXml,
        PowerPoint,
        PowerPointOpenXml,
        Excel,
        ExcelOpenXml,
        Pdf,
        PlainText,
        Mp4,
        Avi,
        Mov,
        Mpeg,
        Quicktime,
        Wav,
        Ogg,
        Flac
    }

    public static readonly MimeTypes ImageMimeTypes = MimeTypes.Jpeg | MimeTypes.Png | MimeTypes.Gif | MimeTypes.Bmp | MimeTypes.Webp | MimeTypes.Tiff | MimeTypes.Svg;
    public static readonly MimeTypes DocumentMimeTypes = MimeTypes.Word | MimeTypes.WordOpenXml | MimeTypes.PowerPoint | MimeTypes.PowerPointOpenXml | MimeTypes.Excel | MimeTypes.ExcelOpenXml | MimeTypes.Pdf | MimeTypes.PlainText;
    public static readonly MimeTypes VideoMimeTypes = MimeTypes.Mp4 | MimeTypes.Avi | MimeTypes.Mov | MimeTypes.Mpeg | MimeTypes.Quicktime;
    public static readonly MimeTypes AudioMimeTypes = MimeTypes.Mpeg | MimeTypes.Wav | MimeTypes.Ogg | MimeTypes.Flac;

    private static readonly Dictionary<MimeTypes, string> mimeTypeMap = new()
    {
        { MimeTypes.Jpeg, "image/jpeg" },
        { MimeTypes.Png, "image/png" },
        { MimeTypes.Gif, "image/gif" },
        { MimeTypes.Bmp, "image/bmp" },
        { MimeTypes.Webp, "image/webp" },
        { MimeTypes.Tiff, "image/tiff" },
        { MimeTypes.Svg, "image/svg+xml" },
         { MimeTypes.Word, "application/msword" },
        { MimeTypes.WordOpenXml, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { MimeTypes.PowerPoint, "application/vnd.ms-powerpoint" },
        { MimeTypes.PowerPointOpenXml, "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
        { MimeTypes.Excel, "application/vnd.ms-excel" },
        { MimeTypes.ExcelOpenXml, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { MimeTypes.Pdf, "application/pdf" },
        { MimeTypes.PlainText, "text/plain" },
        { MimeTypes.Mp4, "video/mp4" },
        { MimeTypes.Avi, "video/avi" },
        { MimeTypes.Mov, "video/mov" },
        { MimeTypes.Mpeg, "video/mpeg" },
        { MimeTypes.Quicktime, "video/quicktime" },
        { MimeTypes.Wav, "audio/wav" },
        { MimeTypes.Ogg, "audio/ogg" },
        { MimeTypes.Flac, "audio/flac" }
    };

    public static string? ToMimeType(this MimeTypes mimeType) => mimeTypeMap.GetValueOrDefault(mimeType);
    public static MimeTypes? FromMimeType(string mimeType) => mimeTypeMap.FirstOrDefault(kvp => kvp.Value == mimeType).Key;
    private static bool IsMimeTypeOfCategory(string mimeType, MimeTypes categoryMimeTypes)
    {
        var mimeTypeEnum = FromMimeType(mimeType);
        if (mimeTypeEnum == null)
        {
            return false;
        }
        return categoryMimeTypes.HasFlag(mimeTypeEnum);
    }

    public static bool IsImageMimeType(string mimeType) => IsMimeTypeOfCategory(mimeType, ImageMimeTypes);
    public static bool IsDocumentMimeType(string mimeType) => IsMimeTypeOfCategory(mimeType, DocumentMimeTypes);
    public static bool IsVideoMimeType(string mimeType) => IsMimeTypeOfCategory(mimeType, VideoMimeTypes);
    public static bool IsAudioMimeType(string mimeType) => IsMimeTypeOfCategory(mimeType, AudioMimeTypes);
}