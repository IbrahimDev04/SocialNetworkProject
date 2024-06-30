using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Smo;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace SocialNetworkApp.Business.Hubs;

public class ChatHub(AppDbContext _context) : Hub
{

    private static ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        UserConnections[Context.ConnectionId] = userId;
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        UserConnections.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }



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

    public async Task SendMessageAsync(string message, string RecieveUserId, string UserId)
    {
        AppUser recieveUser = await _context.appUsers.FirstOrDefaultAsync(up => up.Id == RecieveUserId);

        AppUser currentUser = await _context.appUsers.FirstOrDefaultAsync(up => up.Id == UserId);

         string userId = currentUser.Id;

        ChatData chatData = new ChatData
        {
            Message = message,
            FromId = userId,
            ToId = RecieveUserId
        };



        await _context.chatDatas.AddAsync(chatData);
        await _context.SaveChangesAsync();

        await Clients.User(RecieveUserId).SendAsync("recieveMessage",message, userId);
        
    }

    public async Task CreateGroup(string groupName, string UserId)
    {
        await Groups.AddToGroupAsync(UserId, groupName);

        GroupCreate group = new GroupCreate
        {
            GroupName = groupName,
            GroupCreatorId = UserId
        };

       
        await _context.groupCreates.AddAsync(group);
        await _context.SaveChangesAsync();


        GroupMember member = new GroupMember
        {
            UserId = UserId,
            GroupId = group.Id

        };

        await _context.groupMembers.AddAsync(member);
        await _context.SaveChangesAsync();


    }

    public async Task AddUserToGroup(string groupId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);  
    }


    public async Task SendMessageToGroup(string groupId, string message, string userId)
    {

        var user = await _context.userProfiles.Include(up => up.User).FirstOrDefaultAsync(up => up.UserId == userId); 

        await Clients.Group(groupId).SendAsync("recieveMessageGroup", message, userId, user.User.UserName, user.ProfilePhoto);

        var member = await _context.groupMembers.FirstOrDefaultAsync(gm => gm.UserId == userId && gm.GroupId.ToString() == groupId);

        GroupChatData chat = new GroupChatData
        {
            GroupMemberId = member.Id,
            Message = message,
        };

        await _context.groupChatDatas.AddAsync(chat);
        await _context.SaveChangesAsync();


    }


}
