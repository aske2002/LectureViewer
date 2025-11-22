namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class KeywordOccurrenceConfiguration : IEntityTypeConfiguration<TranscriptKeywordOccurrence>
{
    public void Configure(EntityTypeBuilder<TranscriptKeywordOccurrence> builder)
    {
        builder.HasOne(ltko => ltko.TranscriptKeyword).WithMany(ltk => ltk.Occurrences).HasForeignKey(ltko => ltko.TranscriptKeywordId);
    }
}
