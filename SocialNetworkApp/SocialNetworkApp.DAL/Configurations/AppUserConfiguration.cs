using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasMany(e => e.RelationshipsUser)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.RelationshipsRelUser)
            .WithOne(e => e.RelationalUser)
            .HasForeignKey(e => e.RelationalUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.UserFriendsSender)
            .WithOne(e => e.UserFollowing)
            .HasForeignKey(e => e.UserFollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.UserFriendsTaker)
            .WithOne(e => e.FriendFollowing)
            .HasForeignKey(e => e.FriendFollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.ChatDatasFrom)
            .WithOne(e => e.From)
            .HasForeignKey(e => e.FromId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.ChatDatasTo)
            .WithOne(e => e.To)
            .HasForeignKey(e => e.ToId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.PostStats)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.GroupCreates)
            .WithOne(e => e.GroupCreator)
            .HasForeignKey(e => e.GroupCreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.GroupMembers)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
