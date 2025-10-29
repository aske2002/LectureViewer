namespace StrejsApi.Infrastructure.Databases.Trips.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using backend.Domain.Entities;
internal class CourseInviteLinkConfiguration : IEntityTypeConfiguration<CourseInviteLink>
{
    public void Configure(EntityTypeBuilder<CourseInviteLink> builder)
    {
        builder.HasOne(i => i.Course).WithMany(c => c.InviteLinks).HasForeignKey(i => i.CourseId);
    }
}

