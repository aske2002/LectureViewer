namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
internal class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.HasOne(m => m.Destination).WithMany(r => r.Trips).HasForeignKey(r => r.DestinationId);
        builder.HasOne(m => m.ClassYear).WithMany(r => r.Trips).HasForeignKey(r => r.ClassYearId);
        builder.HasMany(m => m.DescriptionParts).WithOne(r => r.Trip).HasForeignKey(r => r.TripId);
    }
}
