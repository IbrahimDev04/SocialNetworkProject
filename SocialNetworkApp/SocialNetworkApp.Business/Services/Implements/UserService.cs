using Microsoft.AspNetCore.Identity;
using SocialNetworkApp.Business.Exceptions.AppUser;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.Account;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.Business.Services.Implements;

public class UserService(IAppUserRepository _repo, IUserProfileService _userProfileService, IUserSettingsService _userSettingsService) : IUserService
{


    public async Task<IdentityResult> RegisteredAsync(RegisterCurier registerVM)
    {
        AppUser user = new AppUser
        {
            UserName = registerVM.RegisterVM.Username,
            Email = registerVM.RegisterVM.Email,
        };

        

        var result = await _repo.RegisteredAsync(user, registerVM.RegisterVM.Password);

        await _userProfileService.CreateAsync(registerVM.ChangableUserProfileVM, user.Id);

        await _userSettingsService.CreateAsync(registerVM.ChangableUserSettingsVM, user.Id);

        return result;
    }


    public async Task<AppUser> IsExistsUsernameOrEmail(string UserNameOrEmail)
    {
        var user = await _repo.FindByNameAsync(UserNameOrEmail);
        if (user == null)
        {
            user = await _repo.FindByEmailAsync(UserNameOrEmail);
            if (user == null)
            {
                throw new WrongUsernameOrEmailException();
            }
            return user;
        }
        return user;

    }

    public Task CheckSelectionOverrun(AppUser user)
    {
        throw new SelectionOverrunException("You try more time. In that case, you must wait for " + user.LockoutEnd.Value.ToString("HH:mm:ss"));
    }

    public async Task<SignInResult> LoginInAsync(LoginVM vm, AppUser user)
    {

        await _repo.CheckPasswordSignInAsync(user, vm.Password);
        return await _repo.UserSignInAsync(user, vm.Password, vm.RememberMe);
    }
}
