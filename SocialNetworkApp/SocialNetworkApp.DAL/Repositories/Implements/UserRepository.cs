using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.DAL.Repositories.Implements;

public class UserRepository<TIdentity>(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : IUserRepository<TIdentity> where TIdentity : AppUser, new()
{
    public async Task CheckPasswordSignInAsync(AppUser user, string password)
    {
        await _signInManager.CheckPasswordSignInAsync(user, password, true);
    }

    public async Task<AppUser> FindByEmailAsync(string Email)
    {
        return await _userManager.FindByEmailAsync(Email);
    }

    public async Task<AppUser> FindByNameAsync(string Username)
    {
        return await _userManager.FindByNameAsync(Username);
    }

    public async Task<IdentityResult> RegisteredAsync(TIdentity identity, string password)
    {
        return await _userManager.CreateAsync(identity, password);
    }

    public async Task<SignInResult> UserSignInAsync(AppUser user, string password, bool rememberMe)
    {
        return await _signInManager.PasswordSignInAsync(user, password, rememberMe, true);
    }
}
