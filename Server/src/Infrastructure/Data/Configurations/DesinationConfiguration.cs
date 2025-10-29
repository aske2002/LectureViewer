namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
internal class DestinationConfiguration : IEntityTypeConfiguration< Destination>
{
    public void Configure(EntityTypeBuilder<Destination> builder)
    {
        builder.HasMany(m => m.Trips).WithOne(r => r.Destination).HasForeignKey(r => r.DestinationId);
        builder.HasOne(m => m.Country).WithMany(r => r.Destinations).HasForeignKey(r => r.CountryId);
    }
}
