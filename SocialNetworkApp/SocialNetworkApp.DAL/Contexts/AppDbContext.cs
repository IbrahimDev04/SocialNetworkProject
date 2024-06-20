using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.Core.Entities.Commons;
using SocialNetworkApp.DAL.Configurations;

namespace SocialNetworkApp.DAL.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> appUsers { get; set; }
    public DbSet<Categories> categories { get; set; }
    public DbSet<PostComments> postComments { get; set; }
    public DbSet<PostFavorites> postFavorites { get; set; }
    public DbSet<PostStats> postStats { get; set; }
    public DbSet<PostTags> postTags { get; set; }
    public DbSet<Relationship> relationships { get; set; }
    public DbSet<RelationshipType> relationshipTypes { get; set; }
    public DbSet<UserFriends> userFriends { get; set; }
    public DbSet<UserLocation> userLocations { get; set; }
    public DbSet<UserPosts> userPosts { get; set; }
    public DbSet<UserProfile> userProfiles { get; set; }
    public DbSet<UserSettings> userSettings { get; set; }
    public DbSet<UserStories> userStories { get; set; }
    public DbSet<ViewerStory> viewerStories { get; set; }
    public DbSet<ChatData> chatDatas { get; set; }


    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {


        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        ((BaseEntity)entry.Entity).CreatedTime = DateTime.Now;
                        ((BaseEntity)entry.Entity).IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        ((BaseEntity)entry.Entity).UpdatedTime = DateTime.Now;
                        break;
                }
            }

        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(PostCommentConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(PostTagConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(RelationshipTypeConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(UserLocationConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(UserPostConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(UserProfileConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(AppUserConfiguration).Assembly);

        base.OnModelCreating(builder);
    }
}
