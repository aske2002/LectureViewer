namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

internal class CourseInstructorConfiguration : IEntityTypeConfiguration<CourseInstructor>
{
    public void Configure(EntityTypeBuilder<CourseInstructor> builder)
    {
        builder.HasOne(ci => ci.Instructor).WithMany().HasForeignKey(ci => ci.InstructorId);
        builder.HasOne(ci => ci.Course).WithMany(c => c.Instructors).HasForeignKey(ci => ci.CourseId);
    }
}
