using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.PostUser;
using SocialNetworkApp.Business.ViewModels.UserProfile;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class AnotherUserCurier
{
    public string UserId { get; set; }
    public bool Status { get; set; }
    public bool FollowedStatus { get; set; }
    public bool IsForYou { get; set; }
    public List<GetAnotherUserProfileVM> GetAnotherUserProfileVM { get; set; }
    public List<GetUserVM> GetUserVM { get; set; }
    public List<GetPostsVM>? getPostsVM { get; set; }
    public int FriendCount { get; set; }
}
