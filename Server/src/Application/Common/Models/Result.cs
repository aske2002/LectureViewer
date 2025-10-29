namespace backend.Application.Common.Models;

public class Result
{
    internal Result(bool succeeded, Dictionary<string, string[]> errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public bool Succeeded { get; init; }

    public Dictionary<string, string[]> Errors { get; init; }

    public static Result Success()
    {
        return new Result(true, new Dictionary<string, string[]>());
    }

    public static Result Failure(Dictionary<string, string[]> errors)
    {
        return new Result(false, errors);
    }
}
