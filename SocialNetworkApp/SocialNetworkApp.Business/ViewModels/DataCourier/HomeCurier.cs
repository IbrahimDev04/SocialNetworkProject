using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.UserProfile;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class HomeCurier
{
    public GetUserProfileVM userProfileVM { get; set; }
    public GetUserVM userVM { get; set; }
}
