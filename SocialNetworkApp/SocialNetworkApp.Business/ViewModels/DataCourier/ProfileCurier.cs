using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.PostUser;
using SocialNetworkApp.Business.ViewModels.UserProfile;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class ProfileCurier
{
    public GetUserProfileAllVM ProfileAllVM { get; set; }
    public GetUserVM userVM { get; set; }
    public List<GetUserVM> GetUserVM { get; set; }
    public List<GetPostsVM>? getPostsVM { get; set; }
    public List<GetAnotherUserProfileVM> GetAnotherUserProfileVM { get; set; }

    public int FriendCount { get; set; }
}
