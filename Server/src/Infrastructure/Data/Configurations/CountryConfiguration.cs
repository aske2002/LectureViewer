namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;

internal class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasMany(m => m.Destinations).WithOne(r => r.Country).HasForeignKey(r => r.CountryId);
        builder.HasIndex(m => m.IsoCode).IsUnique();
    }
}
