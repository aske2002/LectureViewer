namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class LectureContentConfiguration : IEntityTypeConfiguration<LectureContent>
{
    public void Configure(EntityTypeBuilder<LectureContent> builder)
    {
        builder.HasOne(lc => lc.Lecture).WithMany(l => l.Contents).HasForeignKey(lc => lc.LectureId);
        builder.HasOne(lc => lc.Transcript).WithMany().HasForeignKey(lc => lc.TranscriptId);
        builder.HasOne(lc => lc.Resource).WithMany().HasForeignKey(lc => lc.ResourceId);
        builder.HasOne(lc => lc.Document).WithMany().HasForeignKey(lc => lc.DocumentId);
        builder.Property(lc => lc.ContentType).HasConversion<string>();
    }
}
