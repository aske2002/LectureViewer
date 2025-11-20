namespace backend.Infrastructure.MediaProcessing.Transcription.Models;

public class WhisperOutput
{
    public string Systeminfo { get; set; } = default!;
    public ModelInfo Model { get; set; } = default!;
    public ParamsInfo Params { get; set; } = default!;
    public ResultInfo Result { get; set; } = default!;
    public List<TranscriptionItem> Transcription { get; set; } = default!;
}

public class ModelInfo
{
    public string Type { get; set; } = default!;
    public bool Multilingual { get; set; }
    public int Vocab { get; set; }
    public AudioInfo Audio { get; set; } = default!;
    public TextInfo Text { get; set; } = default!;
    public int Mels { get; set; }
    public int Ftype { get; set; }
}

public class AudioInfo
{
    public int Ctx { get; set; }
    public int State { get; set; }
    public int Head { get; set; }
    public int Layer { get; set; }
}

public class TextInfo
{
    public int Ctx { get; set; }
    public int State { get; set; }
    public int Head { get; set; }
    public int Layer { get; set; }
}

public class ParamsInfo
{
    public string Model { get; set; } = default!;
    public string Language { get; set; } = default!;
    public bool Translate { get; set; }
}

public class ResultInfo
{
    public string Language { get; set; } = default!;
}

public class TranscriptionItem
{
    public TimestampInfo Timestamps { get; set; } = default!;
    public OffsetInfo Offsets { get; set; } = default!;
    public string Text { get; set; } = default!;
    public List<TokenInfo> Tokens { get; set; } = default!;
}

public class TimestampInfo
{
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
}

public class OffsetInfo
{
    public int From { get; set; }
    public int To { get; set; }
}

public class TokenInfo
{
    public string Text { get; set; } = default!;
    public TimestampInfo Timestamps { get; set; } = default!;
    public OffsetInfo Offsets { get; set; } = default!;
    public int Id { get; set; }
    public double P { get; set; }
    public double T_dtw { get; set; }
}