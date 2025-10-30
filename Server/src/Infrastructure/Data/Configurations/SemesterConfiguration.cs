namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
internal class SemesterConfiguration : IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> builder)
    {
        builder.HasMany(m => m.Courses).WithOne(r => r.Semester).HasForeignKey(r => r.SemesterId);
    }
}