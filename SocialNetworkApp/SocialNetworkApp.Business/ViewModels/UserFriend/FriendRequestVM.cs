using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.Business.ViewModels.UserFriend;

public class FriendRequestVM
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public bool IsFriend { get; set; }
}
