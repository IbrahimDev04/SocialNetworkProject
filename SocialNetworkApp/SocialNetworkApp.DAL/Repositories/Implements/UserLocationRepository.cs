using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using SocialNetworkApp.DAL.Repositories.Interfaces;

namespace SocialNetworkApp.DAL.Repositories.Implements;

public class UserLocationRepository : Repository<UserLocation>, IUserLocationRepository
{
    public UserLocationRepository(AppDbContext _context) : base(_context)
    {
    }
}
