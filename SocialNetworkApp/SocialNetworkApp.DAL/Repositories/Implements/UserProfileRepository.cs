using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.DAL.Repositories.Implements;

public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
{
    public UserProfileRepository(AppDbContext _context) : base(_context)
    {
    }
}
