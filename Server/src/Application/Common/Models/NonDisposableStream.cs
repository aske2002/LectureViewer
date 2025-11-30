namespace Application.Common.Models;

public class NonDisposableStreamContent : StreamContent
{
    public NonDisposableStreamContent(Stream stream) : base(stream) { }

    protected override void Dispose(bool disposing)
    {
        // Do NOT dispose the underlying stream
        base.Dispose(false);
    }
}