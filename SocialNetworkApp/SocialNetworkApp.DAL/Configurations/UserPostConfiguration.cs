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

        builder.Property(up => up.Status)
            .IsRequired();
    }
}
