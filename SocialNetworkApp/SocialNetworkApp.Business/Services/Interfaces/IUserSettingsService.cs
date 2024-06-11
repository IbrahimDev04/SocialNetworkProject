using SocialNetworkApp.Business.ViewModels;
using SocialNetworkApp.Business.ViewModels.UserSetting;

namespace SocialNetworkApp.Business.Services.Interfaces
{
    public interface IUserSettingsService
    {
        public Task CreateAsync(ChangableUserSettingsVM userSettingsVM ,string Id);
    }
}
