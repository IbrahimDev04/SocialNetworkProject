using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.UserProfile;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.Business.Services.Implements;

public class UserProfileService(IUserProfileRepository _repo) : IUserProfileService
{
    public async Task CreateAsync(ChangableUserProfileVM userProfileVM, string userId)
    {
        UserProfile user = new UserProfile
        {
            Id = userProfileVM.Id,
            Name = userProfileVM.Name,
            Surname = userProfileVM.Surname,
            Gender = "sgsdg",
            UserId = userId
        };

        await _repo.CreateAsync(user);
        await _repo.SaveAsync();
    }
}
