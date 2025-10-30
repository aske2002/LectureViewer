using backend.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors
                .Select(e => new KeyValuePair<string, string[]>(e.Code, [e.Description]))
                .GroupBy(e => e.Key)
                .ToDictionary(g => g.Key, g => g.SelectMany(e => e.Value).ToArray()));
    }
}
