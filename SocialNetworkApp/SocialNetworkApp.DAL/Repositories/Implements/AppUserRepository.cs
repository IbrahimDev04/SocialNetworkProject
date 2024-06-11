using Microsoft.AspNetCore.Identity;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.DAL.Repositories.Implements;

public class AppUserRepository : UserRepository<AppUser>, IAppUserRepository
{
    public AppUserRepository(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : base(_userManager, _signInManager)
    {
    }
}
