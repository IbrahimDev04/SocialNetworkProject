using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Configurations;

public class UserPostConfiguration : IEntityTypeConfiguration<UserPosts>
{
    public void Configure(EntityTypeBuilder<UserPosts> builder)
    {
        builder.Property(up => up.Description)
            .IsRequired()
            .HasMaxLength(320);

        builder.HasMany(up => up.PostStats)
            .WithOne(up => up.Post)
            .HasForeignKey(up => up.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(up => up.PostFavorites)
            .WithOne(up => up.Post)
            .HasForeignKey(up => up.PostId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasMany(up => up.PostComments)
            .WithOne(up => up.UserPosts)
            .HasForeignKey(up => up.UserPostsId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}
