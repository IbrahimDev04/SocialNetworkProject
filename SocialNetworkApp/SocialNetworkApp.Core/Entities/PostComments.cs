namespace SocialNetworkApp.Core.Entities;

public class PostComments : BaseEntity
{
    public string Comment { get; set; }

    public string UserId { get; set; }
    public AppUser? User { get; set; }

    public Guid UserPostsId { get; set; }
    public UserPosts? UserPosts { get; set; }

}
