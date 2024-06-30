using SocialNetworkApp.Business.ViewModels.Account;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.UserLocation;
using SocialNetworkApp.Business.ViewModels.UserProfile;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class SettingsCurier
{
    public UserProfileSettingsVM? UserProfileVM { get; set; }

    public ChangePasswordVM? ChangePasswordVM { get; set; }

    public ChangableUserLocationVM? ChangableUserLocationVM { get; set; }

    public GetUserProfileUpdateVM GetUserProfileUpdateVM { get; set; } = new GetUserProfileUpdateVM();
}
