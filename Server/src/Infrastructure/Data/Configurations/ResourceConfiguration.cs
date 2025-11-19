namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
internal class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasOne(r => r.ParentResource)
            .WithMany(r => r.AssociatedResources)
            .HasForeignKey(r => r.ParentResourceId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(r => r.ThumbnailResource)
            .WithMany()
            .HasForeignKey(r => r.ThumbnailResourceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
