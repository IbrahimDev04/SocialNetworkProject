using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.UserProfile;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class FriendCurier
{
    public List<GetAnotherUserProfileVM> GetAnotherUserProfileVM { get; set; }
    public List<GetUserVM> GetUserVM { get; set; }
}
