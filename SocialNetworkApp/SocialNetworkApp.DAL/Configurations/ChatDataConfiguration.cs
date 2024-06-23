using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Configurations;

public class ChatDataConfiguration : IEntityTypeConfiguration<ChatData>
{
    public void Configure(EntityTypeBuilder<ChatData> builder)
    {
        builder.Property(c => c.Message)
            .IsRequired();
    }
}
