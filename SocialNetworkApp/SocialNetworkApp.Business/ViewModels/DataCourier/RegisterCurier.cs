using SocialNetworkApp.Business.ViewModels.Account;
using SocialNetworkApp.Business.ViewModels.UserLocation;
using SocialNetworkApp.Business.ViewModels.UserProfile;
using SocialNetworkApp.Business.ViewModels.UserSetting;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class RegisterCurier
{
    public RegisterVM RegisterVM { get; set; } = new RegisterVM();
    public ChangableUserProfileVM ChangableUserProfileVM { get; set; } = new ChangableUserProfileVM();
    public ChangableUserSettingsVM ChangableUserSettingsVM { get; set; } = new ChangableUserSettingsVM();
    public ChangableUserLocationVM ChangableUserLocationVM { get; set ; } = new ChangableUserLocationVM();
}
