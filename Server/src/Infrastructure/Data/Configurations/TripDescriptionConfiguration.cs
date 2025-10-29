namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class TripDescriptionConfiguration : IEntityTypeConfiguration<TripDescription>
{
    public void Configure(EntityTypeBuilder<TripDescription> builder)
    {
        builder.HasOne(m => m.Trip).WithMany(r => r.DescriptionParts).HasForeignKey(r => r.TripId);
    }
}
