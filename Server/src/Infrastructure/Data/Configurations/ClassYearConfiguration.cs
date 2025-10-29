namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class ClassYearConfiguration : IEntityTypeConfiguration<ClassYear>
{
    public void Configure(EntityTypeBuilder<ClassYear> builder)
    {
        builder.HasMany(m => m.Trips).WithOne(r => r.ClassYear).HasForeignKey(r => r.ClassYearId);
    }
}
