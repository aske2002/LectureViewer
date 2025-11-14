namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class LectureTranscriptKeywordConfiguration : IEntityTypeConfiguration<LectureTranscriptKeyword>
{
    public void Configure(EntityTypeBuilder<LectureTranscriptKeyword> builder)
    {
        builder.HasOne(k => k.LectureTranscript).WithMany(lc => lc.Keywords).HasForeignKey(k => k.LectureTranscriptId);
    }
}
