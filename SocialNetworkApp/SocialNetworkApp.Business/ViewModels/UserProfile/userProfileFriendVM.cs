using SocialNetworkApp.Business.ViewModels.AppUser;
namespace SocialNetworkApp.Business.ViewModels.UserProfile;

public class userProfileFriendVM
{
    public GetUserVM User { get; set; }
    public bool IsFollowing { get; set; }
}
