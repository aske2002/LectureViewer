using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Services;

public interface IKeywordService
{
    Task<ICollection<Keyword>> CreateOrGetKeywordsAsync(IEnumerable<string> keywordTexts, CancellationToken cancellationToken);
}

public class KeywordService : IKeywordService
{
    private readonly IApplicationDbContext _dbContext;

    public KeywordService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<Keyword>> CreateOrGetKeywordsAsync(IEnumerable<string> keywordTexts, CancellationToken cancellationToken)
    {
        var normalized = keywordTexts
             .Where(k => !string.IsNullOrWhiteSpace(k))
             .Select(k => k.Trim().ToLower())
             .Distinct()
             .ToList();

        // Get all existing keywords at once
        var existing = await _dbContext.Keywords
            .Where(k => normalized.Contains(k.Text))
            .ToListAsync(cancellationToken);

        var existingSet = existing.Select(k => k.Text).ToHashSet();

        // Create missing
        var newKeywords = normalized
            .Where(t => !existingSet.Contains(t))
            .Select(t => new Keyword { Text = t })
            .ToList();

        if (newKeywords.Count > 0)
        {
            _dbContext.Keywords.AddRange(newKeywords);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return existing.Concat(newKeywords).ToList();
    }
}