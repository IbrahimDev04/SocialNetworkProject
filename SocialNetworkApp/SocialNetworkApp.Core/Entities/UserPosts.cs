namespace SocialNetworkApp.Core.Entities;

public class UserPosts : BaseEntity
{
    public string Description { get; set; }
    public string? ImageOrVideoUrl { get; set; }
    public bool Status { get; set; }

    public string CategoryId { get; set; }
    public Categories? Category { get; set; }

    public string UserId { get; set; }
    public AppUser? User { get; set; }

    public ICollection<PostFavorites>? PostFavorites { get; set; }
    public ICollection<PostComments>? PostComments { get; set; }
    public ICollection<PostStats>? PostStats { get; set; }
    public ICollection<PostTags>? PostTags { get; set; }

}
