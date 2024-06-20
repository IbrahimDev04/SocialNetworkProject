using Microsoft.AspNetCore.Identity;
using SocialNetworkApp.Business.Exceptions.AppUser;
using SocialNetworkApp.Business.ViewModels.Account;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.Business.Services.Interfaces;

public interface IUserService
{
    public Task<AppUser> RegisteredAsync(RegisterCurier registerVM);

    public Task<SignInResult> LoginInAsync(LoginVM vm, AppUser user);

    public Task<AppUser> IsExistsUsernameOrEmail(string UserNameOrEmail);

    public Task CheckSelectionOverrun(AppUser user);
}
