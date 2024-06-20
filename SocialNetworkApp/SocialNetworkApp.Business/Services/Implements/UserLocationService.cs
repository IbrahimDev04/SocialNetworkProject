using Microsoft.SqlServer.Management.Smo;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.UserLocation;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.Business.Services.Implements;

public class UserLocationService(IUserLocationRepository _repo) : IUserLocationService
{
    public async Task CreateAsync(ChangableUserLocationVM userProfileVM, string userId)
    {
        UserLocation userLocationVM = new UserLocation
        {
            UserId = userId,
            Country = userProfileVM.Country,
            City = userProfileVM.City,
            State = userProfileVM.State,
        };

        await _repo.CreateAsync(userLocationVM);
        await _repo.SaveAsync();



    }
}
