namespace SocialNetworkApp.Core.Entities;

public class PostStats : BaseEntity
{
    public int LikeCount { get; set; }

    public string PostId { get; set; }
    public UserPosts? Post { get; set; }
}
