namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasMany(c => c.InviteLinks).WithOne(l => l.Course).HasForeignKey(i => i.CourseId);
        builder.HasMany(c => c.Instructors).WithOne().HasForeignKey(ci => ci.CourseId);
        builder.HasMany(c => c.Lectures).WithOne(l => l.Course).HasForeignKey(l => l.CourseId);
        builder.HasOne(c => c.Semester).WithMany(s => s.Courses).HasForeignKey(c => c.SemesterId);
        builder.HasMany(c => c.Keywords).WithOne(k => k.Course).HasForeignKey(k => k.CourseId);
        builder.HasMany(c => c.Enrollments).WithOne(e => e.Course).HasForeignKey(e => e.CourseId);
        builder.HasMany(c => c.Lectures).WithOne(l => l.Course).HasForeignKey(l => l.CourseId);
        builder.HasMany(c => c.Keywords).WithOne(ck => ck.Course).HasForeignKey(ck => ck.CourseId);
        builder.OwnsOne(b => b.Colour);
    }
}
