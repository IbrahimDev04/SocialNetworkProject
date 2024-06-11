using SocialNetworkApp.Business.ViewModels.UserProfile;

namespace SocialNetworkApp.Business.Services.Interfaces;

public interface IUserProfileService
{
    public Task CreateAsync(ChangableUserProfileVM userProfileVM, string userId);
}
