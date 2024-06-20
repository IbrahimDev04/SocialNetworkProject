namespace SocialNetworkApp.Business.ViewModels.UserFriend;

public class UserProfileVM
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public bool IsFriend { get; set; }
    public bool FriendRequestSent { get; set; }
}
