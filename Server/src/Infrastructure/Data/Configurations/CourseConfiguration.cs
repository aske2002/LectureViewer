namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
internal class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasMany(c => c.Lectures).WithOne(l => l.Course).HasForeignKey(r => r.CourseId);
        builder.HasMany(c => c.InviteLinks).WithOne(l => l.Course).HasForeignKey(i => i.CourseId);

    }
}
