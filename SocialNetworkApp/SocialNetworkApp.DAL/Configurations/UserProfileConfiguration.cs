using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.Property(up => up.Name)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(up => up.Surname)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(up => up.Gender)
            .IsRequired();

        builder.Property(up => up.About)
            .HasMaxLength(640);

        builder.Property(up => up.CurrentStudyUni)
            .HasMaxLength(120);

        builder.Property(up => up.CurrentWorkAt)
            .HasMaxLength(120);
    }
}
