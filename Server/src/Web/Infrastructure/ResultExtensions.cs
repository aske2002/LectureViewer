using backend.Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

public static class ResultExtensions
{
    public static Results<Ok, ValidationProblem> ToHttpResult(this Result result)
    {
        return result.Succeeded
            ? TypedResults.Ok()
            : TypedResults.ValidationProblem(result.Errors);
    }
}