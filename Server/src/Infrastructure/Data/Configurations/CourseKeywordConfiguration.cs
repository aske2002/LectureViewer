namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class CourseKeywordConfiguration : IEntityTypeConfiguration<CourseKeyword>
{
    public void Configure(EntityTypeBuilder<CourseKeyword> builder)
    {
        builder.HasOne(ck => ck.Course).WithMany(c => c.Keywords).HasForeignKey(ck => ck.CourseId);
        builder.HasOne(ck => ck.Keyword).WithMany(k => k.CourseKeywords).HasForeignKey(ck => ck.KeywordId);
    }
}