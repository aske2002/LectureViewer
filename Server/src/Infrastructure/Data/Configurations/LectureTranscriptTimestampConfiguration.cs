namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class LectureTranscriptTimestampConfiguration : IEntityTypeConfiguration<LectureTranscriptTimestamp>
{
    public void Configure(EntityTypeBuilder<LectureTranscriptTimestamp> builder)
    {
        builder.HasOne(t => t.LectureTranscript).WithMany(lc => lc.Timestamps).HasForeignKey(t => t.LectureTranscriptId);
    }
}
