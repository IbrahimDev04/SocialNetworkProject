using SocialNetworkApp.Business.ViewModels.Account;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.UserLocation;
using SocialNetworkApp.Business.ViewModels.UserProfile;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class SettingsCurier
{
    public UserProfileSettingsVM UserProfileVM { get; set; } = new UserProfileSettingsVM();

    public ChangePasswordVM? ChangePasswordVM { get; set; }

    public ChangableUserLocationVM ChangableUserLocationVM { get; set; } = new ChangableUserLocationVM();
}
