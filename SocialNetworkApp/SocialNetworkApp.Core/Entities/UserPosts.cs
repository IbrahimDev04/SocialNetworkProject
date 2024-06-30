namespace SocialNetworkApp.Core.Entities;

public class UserPosts : BaseEntity
{
    public string Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }


    public string UserId { get; set; }
    public AppUser? User { get; set; }

    public Guid? UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }

    public ICollection<PostFavorites>? PostFavorites { get; set; }
    public ICollection<PostComments>? PostComments { get; set; }
    public ICollection<PostStats>? PostStats { get; set; }
    public ICollection<PostTags>? PostTags { get; set; }

}
