namespace SocialNetworkApp.Core.Entities;

public class UserFriends : BaseEntity
{
    public bool Status { get; set; } = false;
    public string UserFollowingId { get; set; }
    public AppUser? UserFollowing { get; set; }

    public string FriendFollowingId { get; set; }
    public AppUser? FriendFollowing { get; set; }
}
