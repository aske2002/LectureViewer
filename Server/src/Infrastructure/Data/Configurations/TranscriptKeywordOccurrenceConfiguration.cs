namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class KeywordOccurrenceConfiguration : IEntityTypeConfiguration<TranscriptKeywordOccurrence>
{
    public void Configure(EntityTypeBuilder<TranscriptKeywordOccurrence> builder)
    {
        builder.HasOne(ltko => ltko.Keyword).WithMany(ltk => ltk.TranscriptOccurrences).HasForeignKey(ltko => ltko.KeywordId);
        builder.HasOne(ltko => ltko.Transcript).WithMany(t => t.Keywords).HasForeignKey(ltko => ltko.TranscriptId);
    }
}
