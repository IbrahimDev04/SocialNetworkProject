using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Configurations;

public class GroupCreateConfiguration : IEntityTypeConfiguration<GroupCreate>
{
    public void Configure(EntityTypeBuilder<GroupCreate> builder)
    {
        builder.HasMany(e => e.GroupMembers)
            .WithOne(e => e.Group)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
