using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Configurations;

public class PostCommentConfiguration : IEntityTypeConfiguration<PostComments>
{
    public void Configure(EntityTypeBuilder<PostComments> builder)
    {
        builder.Property(pc => pc.Comment)
            .IsRequired()
            .HasMaxLength(640);
    }

}
