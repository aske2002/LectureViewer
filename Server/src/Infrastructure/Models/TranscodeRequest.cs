namespace backend.Infrastructure.Models;

public record TranscodeRequest(string Format, int? Bitrate = null, int? Width = null, int? Height = null, int? Framerate = null, int? AudioChannels = null, int? AudioSampleRate = null);