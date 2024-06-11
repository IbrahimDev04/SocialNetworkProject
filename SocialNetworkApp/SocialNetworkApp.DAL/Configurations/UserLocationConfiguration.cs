using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Configurations;

public class UserLocationConfiguration : IEntityTypeConfiguration<UserLocation>
{
    public void Configure(EntityTypeBuilder<UserLocation> builder)
    {
        builder.Property(ul => ul.Country)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(ul => ul.City)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(ul => ul.State)
            .IsRequired()
            .HasMaxLength(64);

    }
}
