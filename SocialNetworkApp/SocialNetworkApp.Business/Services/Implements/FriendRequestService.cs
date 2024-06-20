using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Smo;
using SocialNetworkApp.Business.Exceptions.Common;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.UserFriend;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;

namespace SocialNetworkApp.Business.Services.Implements;

public class FriendRequestService(AppDbContext _context) : IFriendRequestService
{
    //public async Task FollowUserAsync(string currentUserId, string userIdToFollow)
    //{
    //    var userFriend = new UserFriends
    //    {
    //        UserFollowingId = currentUserId,
    //        FriendFollowingId = userIdToFollow,
    //        FollowingStatus = true
    //    };

    //    _context.UserFriends.Add(userFriend);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task UnfollowUserAsync(string currentUserId, string userIdToUnfollow)
    //{
    //    var userFriend = await _context.UserFriends
    //        .FirstOrDefaultAsync(uf => uf.UserFollowingId == currentUserId && uf.FriendFollowingId == userIdToUnfollow);

    //    if (userFriend != null)
    //    {
    //        _context.UserFriends.Remove(userFriend);
    //        await _context.SaveChangesAsync();
    //    }
    //}

    public async Task SendFollow(string currentUserId, string userIdToFollow)
    {
        var userFriend = new UserFriends
        {
            UserFollowingId = currentUserId,
            FriendFollowingId = userIdToFollow,
            Status = false
        };

        var isSendRequest = await _context.userFriends.AnyAsync(uf => uf.UserFollowingId == currentUserId && uf.FriendFollowingId == userIdToFollow && uf.Status == false);
        var isAlreadyFriend1 = await _context.userFriends.AnyAsync(uf => uf.UserFollowingId == currentUserId && uf.FriendFollowingId == userIdToFollow && uf.Status == true);

        var isHaveRequest = await _context.userFriends.AnyAsync(uf => uf.UserFollowingId == userIdToFollow && uf.FriendFollowingId == currentUserId && uf.Status == false);
        var isAlreadyFriend = await _context.userFriends.AnyAsync(uf => uf.UserFollowingId == userIdToFollow && uf.FriendFollowingId == currentUserId && uf.Status == true);


        if (isSendRequest) throw new NotFountException("Zaten gonderilib.");
        if (isHaveRequest) throw new NotFountException("Bu istifadəçidən dostluq göntərilib. Zəhmət olmasa qəbul edin.");
        if (isAlreadyFriend || isAlreadyFriend1) throw new NotFountException("Zaten dostsunuz.");



        _context.userFriends.Add(userFriend);
        await _context.SaveChangesAsync();
    }

    public async Task AcceptFollow(string currentUserId, string userIdToFollow)
    {
        var user = await _context.userFriends
            .FirstOrDefaultAsync(uf => 
            uf.FriendFollowingId ==userIdToFollow && 
            uf.UserFollowingId == currentUserId
            );


        if (user == null) throw new NotFountException("Tapilmadi");

        if(user.Status == true) throw new NotFountException("Zaten takib olunur.");

        user.Status = true;

        await _context.SaveChangesAsync();
    }

    public async Task UnfollowUserAsync(string currentUserId, string userIdToUnfollow)
    {
        var userFriend = await _context.userFriends
            .FirstOrDefaultAsync(uf =>
            uf.UserFollowingId == userIdToUnfollow && 
            uf.FriendFollowingId == currentUserId
            );
        if (userFriend == null)
        {
             userFriend = await _context.userFriends
                .FirstOrDefaultAsync(uf =>
                uf.UserFollowingId == currentUserId &&
                uf.FriendFollowingId == userIdToUnfollow
            );
        }

        if (userFriend != null)
        {
            if(userFriend.Status == false)
            {
                _context.userFriends.Remove(userFriend);
                
            }
            else
            {
                userFriend.Status = false;
            }
            await _context.SaveChangesAsync();

        }
    }

    public async Task<List<UserFriendRequestVM>> GetUserSender(string currentUserId)
    {
        var users = await _context.userFriends
            .Include(uf => uf.UserFollowing)
            .Where(uf => uf.FriendFollowingId == currentUserId && uf.Status == false)
            .Select(uf => new UserFriendRequestVM
            {
                UserId = uf.UserFollowingId,
                RequsetUserId = uf.FriendFollowingId,
                UserName = uf.UserFollowing.UserName
            })
            .ToListAsync();

        return users;
    }


}
