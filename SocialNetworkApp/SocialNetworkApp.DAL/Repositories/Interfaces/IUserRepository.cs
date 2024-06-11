using Microsoft.AspNetCore.Identity;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.DAL.Repositories.Interfaces;

public interface IUserRepository<TIdentity> where TIdentity : AppUser, new ()
{
    public Task<IdentityResult> RegisteredAsync(TIdentity identity, string password);
    public Task<AppUser> FindByNameAsync(string Username);
    public Task<AppUser> FindByEmailAsync(string Email);
    public Task CheckPasswordSignInAsync(AppUser user, string password);
    public Task<SignInResult> UserSignInAsync(AppUser user, string password, bool rememberMe);

}
