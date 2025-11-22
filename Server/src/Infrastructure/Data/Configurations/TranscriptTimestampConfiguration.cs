namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class TranscriptItemConfiguration : IEntityTypeConfiguration<TranscriptItem>
{
    public void Configure(EntityTypeBuilder<TranscriptItem> builder)
    {
        builder.HasOne(t => t.Transcript).WithMany(lc => lc.Items).HasForeignKey(t => t.TranscriptId);
    }
}
