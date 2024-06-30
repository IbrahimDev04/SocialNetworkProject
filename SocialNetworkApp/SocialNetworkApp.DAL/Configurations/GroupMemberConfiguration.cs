using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Configurations;

public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.HasMany(e => e.GroupChatDatas)
        .WithOne(e => e.GroupMember)
        .HasForeignKey(e => e.GroupMemberId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}

