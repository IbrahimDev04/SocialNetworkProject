

using SocialNetworkApp.Business.ViewModels.UserLocation;

namespace SocialNetworkApp.Business.Services.Interfaces;

public interface IUserLocationService
{
    public Task CreateAsync(ChangableUserLocationVM userProfileVM, string userId);
}
