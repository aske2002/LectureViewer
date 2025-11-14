namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class LectureTranscriptKeywordOccurrenceConfiguration : IEntityTypeConfiguration<LectureTranscriptKeywordOccurrence>
{
    public void Configure(EntityTypeBuilder<LectureTranscriptKeywordOccurrence> builder)
    {
        builder.HasOne(ltko => ltko.LectureTranscriptKeyword).WithMany(ltk => ltk.Occurrences).HasForeignKey(ltko => ltko.LectureTranscriptKeywordId);
    }
}
