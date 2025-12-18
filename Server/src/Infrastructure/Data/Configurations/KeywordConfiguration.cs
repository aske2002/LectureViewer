namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
{
    public void Configure(EntityTypeBuilder<Keyword> builder)
    {
        builder.HasMany(k => k.CourseKeywords).WithOne(ck => ck.Keyword).HasForeignKey(ck => ck.KeywordId);
        builder.HasMany(k => k.SourceJobs).WithMany(j => j.ExtractedKeywords);
        builder.HasMany(k => k.DocumentOccurrences).WithOne(dk => dk.Keyword).HasForeignKey(dk => dk.KeywordId);
        builder.HasMany(k => k.TranscriptOccurrences).WithOne(lo => lo.Keyword).HasForeignKey(lo => lo.KeywordId);
        builder.HasIndex(k => k.Text).IsUnique();
    }
}