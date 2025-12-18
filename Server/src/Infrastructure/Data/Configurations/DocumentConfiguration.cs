namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasOne(d => d.Resource).WithMany().HasForeignKey(d => d.ResourceId);
        builder.HasMany(c => c.DocumentKeywords).WithOne(l => l.Document).HasForeignKey(i => i.DocumentId);
        builder.HasMany(d => d.Pages).WithOne(p => p.Document).HasForeignKey(p => p.DocumentId);
    }
}

internal class DocumentPageConfiguration : IEntityTypeConfiguration<DocumentPage>
{
    public void Configure(EntityTypeBuilder<DocumentPage> builder)
    {
        builder.HasOne(dp => dp.Document).WithMany(d => d.Pages).HasForeignKey(dp => dp.DocumentId);
    }
}

internal class DocumentKeywordConfiguration : IEntityTypeConfiguration<DocumentKeyword>
{
    public void Configure(EntityTypeBuilder<DocumentKeyword> builder)
    {
        builder.HasOne(k => k.Document).WithMany(r => r.DocumentKeywords).HasForeignKey(k => k.DocumentId);
        builder.HasOne(k => k.Keyword).WithMany(k => k.DocumentOccurrences).HasForeignKey(k => k.KeywordId);
    }
}
