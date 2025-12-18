namespace backend.Infrastructure.Models;

public record PositionedTextChunk(
    string Text,
    float X,
    float Y,
    float Width,
    float Height,
    int StartIndexInPage,
    int EndIndexInPage
);

public record PageDetails(
    int PageNumber,
    float Width,
    float Height,
    string Text,
    List<PositionedTextChunk> Chunks
);

public record DocumentDetailsResponse(
    int NumberOfPages,
    string? Title,
    string? Author,
    List<PageDetails> Pages
);

