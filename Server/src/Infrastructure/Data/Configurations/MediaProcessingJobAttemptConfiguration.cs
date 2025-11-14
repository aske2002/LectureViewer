namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class MediaProcessingJobAttemptConfiguration : IEntityTypeConfiguration<MediaProcessingJobAttempt>
{
    public void Configure(EntityTypeBuilder<MediaProcessingJobAttempt> builder)
    {
        builder.Property(a => a.Status).HasConversion<string>();
        builder.HasMany(a => a.Logs).WithOne(l => l.Attempt).HasForeignKey(l => l.AttemptId);
        builder.HasOne(a => a.Job).WithMany(j => j.Attempts).HasForeignKey(a => a.JobId);
    }
}

