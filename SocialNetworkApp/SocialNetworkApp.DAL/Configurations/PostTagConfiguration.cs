using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Configurations;

public class PostTagConfiguration : IEntityTypeConfiguration<PostTags>
{
    public void Configure(EntityTypeBuilder<PostTags> builder)
    {
        builder.Property(pt => pt.TagName)
            .IsRequired()
            .HasMaxLength(64);
    }
}
