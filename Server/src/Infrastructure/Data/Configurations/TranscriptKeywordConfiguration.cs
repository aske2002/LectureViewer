namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class TranscriptKeywordConfiguration : IEntityTypeConfiguration<TranscriptKeyword>
{
    public void Configure(EntityTypeBuilder<TranscriptKeyword> builder)
    {
        builder.HasOne(k => k.Transcript).WithMany(lc => lc.Keywords).HasForeignKey(k => k.TranscriptId);
    }
}
