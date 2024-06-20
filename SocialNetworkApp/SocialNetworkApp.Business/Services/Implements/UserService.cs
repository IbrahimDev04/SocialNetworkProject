using Microsoft.AspNetCore.Identity;

using SocialNetworkApp.Business.Exceptions.AppUser;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.Account;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Repositories.Interfaces;


namespace SocialNetworkApp.Business.Services.Implements;

public class UserService(IUserLocationService _userLocationService, IAppUserRepository _repo, IUserProfileService _userProfileService, UserManager<AppUser> _userManager, IUserSettingsService _userSettingsService) : IUserService
{


    public async Task<AppUser> RegisteredAsync(RegisterCurier registerVM)
    {
        AppUser user = new AppUser
        {
            UserName = registerVM.RegisterVM.Username,
            Email = registerVM.RegisterVM.Email,
        };

        

        var result = await _repo.RegisteredAsync(user, registerVM.RegisterVM.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                throw new RegisterFailedException(error.Description);
            }
        }

        await _userLocationService.CreateAsync(registerVM.ChangableUserLocationVM, user.Id);

        await _userProfileService.CreateAsync(registerVM.ChangableUserProfileVM, user.Id, "imgs/profilePhoto/download.png");

        await _userSettingsService.CreateAsync(registerVM.ChangableUserSettingsVM, user.Id);


        return user;
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



        var check = await _repo.CheckPasswordSignInAsync(user, vm.Password);

        
        

        var signIn =  await _repo.UserSignInAsync(user, vm.Password, vm.RememberMe);

        if (!check.Succeeded)
        {
            throw new WrongUsernameOrEmailException("Username or Password is wrong");
        }

        return signIn;
    }
}
