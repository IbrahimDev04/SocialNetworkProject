using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;

namespace SocialNetworkApp.Business.Services.Implements;

public class FollowService(AppDbContext _context) : IFollowService
{
    public async Task FollowUserAsync(string currentUserId, string userIdToFollow)
    {
        var userFriend = new UserFriends
        {
            UserFollowingId = currentUserId,
            FriendFollowingId = userIdToFollow,
        };

        await _context.userFriends.AddAsync(userFriend);
        await _context.SaveChangesAsync();
    }

    public async Task UnfollowUserAsync(string currentUserId, string userIdToUnfollow)
    {
        var userFriend = await _context.userFriends
            .FirstOrDefaultAsync(uf => uf.UserFollowingId == currentUserId && uf.FriendFollowingId == userIdToUnfollow);

        if (userFriend != null)
        {
            _context.userFriends.Remove(userFriend);
            await _context.SaveChangesAsync();
        }
    }
}
