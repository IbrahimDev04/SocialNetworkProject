using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.DAL.Repositories.Implements;

public class UserSettingRepository : Repository<UserSettings>, IUserSettingRepository
{
    public UserSettingRepository(AppDbContext _context) : base(_context)
    {
    }
}
