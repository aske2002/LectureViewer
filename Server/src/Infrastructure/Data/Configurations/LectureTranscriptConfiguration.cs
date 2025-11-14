namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class LectureTranscriptConfiguration : IEntityTypeConfiguration<LectureTranscript>
{
    public void Configure(EntityTypeBuilder<LectureTranscript> builder)
    {
        builder.HasOne(f => f.Lecture).WithMany(l => l.Transcripts).HasForeignKey(f => f.LectureId);
        builder.HasOne(f => f.Source).WithOne(lc => lc.Transcript).HasForeignKey<LectureTranscript>(f => f.SourceId);
        builder.HasOne(f => f.ArtifactFromJob).WithMany().HasForeignKey(f => f.ArtifactFromJobId);

        builder.HasMany(f => f.Keywords).WithOne(k => k.LectureTranscript).HasForeignKey(k => k.LectureTranscriptId);
        builder.HasMany(f => f.Timestamps).WithOne(t => t.LectureTranscript).HasForeignKey(t => t.LectureTranscriptId);
    }
}
