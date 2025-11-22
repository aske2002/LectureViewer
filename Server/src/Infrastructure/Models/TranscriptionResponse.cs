public enum TranscriptionProvider
{
    LocalWhisper,
    Azure
}

public class TranscriptionResponseTimeStamp
{
    public required TimeSpan From {get; set;}
    public required TimeSpan To {get; set;}
}

public class TranscriptionResponseItem
{
    public required string Text { get; set; }
    public required double Confidence { get; set; }
    public required TranscriptionResponseTimeStamp TimeStamp {get; set;}
}

public class TranscriptionResponse
{
    public required TranscriptionProvider Provider {get; set;}
    public required string ModelName { get; set; }
    public required string Language {get; set;}
    public IEnumerable<TranscriptionResponseItem> Items {get; set;} = new List<TranscriptionResponseItem>();

    public string FullText 
    {
        get
        {
            return string.Join(" ", Items.Select(i => i.Text));
        }
    }
}