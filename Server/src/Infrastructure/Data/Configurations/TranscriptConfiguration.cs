namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class TranscriptConfiguration : IEntityTypeConfiguration<Transcript>
{
    public void Configure(EntityTypeBuilder<Transcript> builder)
    {
        builder.HasOne(f => f.Source).WithMany().HasForeignKey(f => f.SourceId);
        builder.HasOne(f => f.Job).WithOne(j => j.Transcript).HasForeignKey<Transcript>(f => f.JobId);

        builder.HasMany(f => f.Keywords).WithOne(k => k.Transcript).HasForeignKey(k => k.TranscriptId);
        builder.HasMany(f => f.Items).WithOne(t => t.Transcript).HasForeignKey(t => t.TranscriptId);
    }
}
