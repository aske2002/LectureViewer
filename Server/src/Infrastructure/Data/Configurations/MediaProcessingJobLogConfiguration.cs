namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class MediaProcessingJobLogConfiguration : IEntityTypeConfiguration<MediaProcessingJobLog>
{
    public void Configure(EntityTypeBuilder<MediaProcessingJobLog> builder)
    {
        builder.HasOne(j => j.Attempt).WithMany(j => j.Logs).HasForeignKey(j => j.AttemptId);
    }
}
