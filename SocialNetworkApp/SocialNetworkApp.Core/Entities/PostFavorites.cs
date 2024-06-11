namespace SocialNetworkApp.Core.Entities;

public class PostFavorites : BaseEntity
{
    public string PostId { get; set; }
    public UserPosts? Post { get; set; }

    public string UserId { get; set; }
    public AppUser? User { get; set; }
}
