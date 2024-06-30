namespace SocialNetworkApp.Core.Entities;

public class PostStats : BaseEntity
{

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public Guid PostId { get; set; }
    public UserPosts? Post { get; set; }
}
