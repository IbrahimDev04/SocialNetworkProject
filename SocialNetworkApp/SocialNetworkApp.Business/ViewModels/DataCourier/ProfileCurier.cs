using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.UserProfile;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class ProfileCurier
{
    public GetUserProfileAllVM ProfileAllVM { get; set; }
    public GetUserVM userVM { get; set; }
}
