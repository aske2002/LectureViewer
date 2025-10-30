namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
internal class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasIndex(e => e.EntityId)
            .HasDatabaseName("IX_Resources_EntityId");
    }
}
