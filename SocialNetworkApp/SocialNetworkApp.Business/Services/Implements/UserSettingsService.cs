using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.UserSetting;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.Business.Services.Implements;

public class UserSettingsService(IUserSettingRepository _repo) : IUserSettingsService
{
    public async Task CreateAsync(ChangableUserSettingsVM userSettingsVM, string Id)
    {

        UserSettings user = new UserSettings
        {
            ProfileVisbility = userSettingsVM.ProfileVisbility,
            NotifactionsComments = userSettingsVM.NotifactionsComments,
            NotifactionsMessages = userSettingsVM.NotifactionsMessages,
            UserProfileId = Id
        };

        await _repo.CreateAsync(user);
        await _repo.SaveAsync();
    }
}
