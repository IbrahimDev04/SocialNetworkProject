using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Business.ViewModels.UserProfile;

namespace SocialNetworkApp.Business.Services.Interfaces;

public interface IUserProfileService
{

    public Task<bool> CheckImage(ChangableUserProfileVM userProfileVM);
    public Task CreateAsync(ChangableUserProfileVM userProfileVM, string userId, string? fileName);
    public Task<SettingsCurier> GetUserProfileById(string UserId);
    public Task<SettingsCurier> UpdateUserProfileById(SettingsCurier curier, string UserId);
    public Task UpdateUserProfilePhotoById(string UserId, string? fileName);
    public Task UpdateUserLocationById(SettingsCurier curier, string UserId);
}
