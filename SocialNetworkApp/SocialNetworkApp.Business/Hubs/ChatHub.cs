using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Smo;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using System.Security.Claims;

namespace SocialNetworkApp.Business.Hubs;

public class ChatHub(AppDbContext _context) : Hub
{
    public async Task GetNickName(string Username, string UserId)
    {

        var users = await _context.userProfiles
                .Include(up => up.User)
                .Where(up => up.UserId != UserId &&
                _context.userFriends.Any(uf => (((uf.UserFollowingId == UserId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == UserId && uf.UserFollowingId == up.UserId)) && uf.Status == true)))
                .Select(up => new GetUserVM
                {
                    Id = up.UserId,
                    UserName = up.User.UserName,
                    Name = up.Name,
                    Surname = up.Surname,
                    ProfilePhoto = up.ProfilePhoto,
                    Status = _context.userFriends.Any(uf => ((uf.UserFollowingId == UserId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == UserId && uf.UserFollowingId == up.UserId))),
                    FollowedStatus = _context.userFriends.Any(uf => uf.UserFollowingId == UserId && uf.FriendFollowingId == up.UserId && uf.Status == false),
                    IsForYou = _context.userFriends.Any(uf => (((uf.UserFollowingId == UserId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == UserId && uf.UserFollowingId == up.UserId)) && uf.Status == true)),
                }).ToListAsync();

        List<string> user = new List<string>();

        users.ForEach(u => user.Add(u.Id));

        IReadOnlyList<string> clients = user;

        await Clients.Users(clients).SendAsync("ClientJoined", Username);
    }


}
