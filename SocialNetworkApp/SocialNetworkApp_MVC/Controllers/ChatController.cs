using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Smo;
using NuGet.Packaging.Signing;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.ChatData;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Business.ViewModels.GroupChatData;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace SocialNetworkApp_MVC.Controllers
{
    public class ChatController(AppDbContext _context, UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index(string? recieveId)
        {
            var users = await _context.userProfiles
                .Include(up => up.User)
                .Where(up => up.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value &&
                _context.userFriends.Any(uf => (((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId)) && uf.Status == true)))
                .Select(up => new GetUserVM
                {
                    Id = up.UserId,
                    UserName = up.User.UserName,
                    Name = up.Name,
                    Surname = up.Surname,
                    ProfilePhoto = up.ProfilePhoto,
                    Status = _context.userFriends.Any(uf => ((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId))),
                    FollowedStatus = _context.userFriends.Any(uf => uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId && uf.Status == false),
                    IsForYou = _context.userFriends.Any(uf => (((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId)) && uf.Status == true)),
                    
                }).ToListAsync();

            var sendChat = await _context.chatDatas
                .Where(cd => ((cd.ToId == recieveId && cd.FromId == User.FindFirst(ClaimTypes.NameIdentifier).Value) || (cd.FromId == recieveId && cd.ToId == User.FindFirst(ClaimTypes.NameIdentifier).Value)))
                .Select(cd => new GetChatDataVM
                {
                    Message = cd.Message,
                    ToId = cd.ToId,
                    FromId = cd.FromId,
                    Datetime = cd.CreatedTime.ToString("dd MMM yyyy"),
                }).ToListAsync();


            var user = await _context.userFriends.FirstOrDefaultAsync(uf => (uf.UserFollowingId == recieveId && uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value) || (uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == recieveId));

            var userprofile = user != null ? (user.UserFollowingId == recieveId ? await _context.userProfiles.Include(uf => uf.User).FirstOrDefaultAsync(up => up.UserId == user.UserFollowingId) : await _context.userProfiles.Include(uf => uf.User).FirstOrDefaultAsync(up => up.UserId == user.FriendFollowingId)) : null;

            HomeCurierFriends homeCurier = new HomeCurierFriends
            {
                userVM = users,
                sendChatDataVM = sendChat,
                recieveId = recieveId != null ? recieveId : "Null",
                FullName = userprofile != null ? userprofile.User.UserName : "Null",
                ProfilePhote = userprofile != null ? userprofile.ProfilePhoto : "Null",
                Name = userprofile != null ? userprofile.Name + " " + userprofile.Surname : "Null",
                CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                CurrentUserName = _userManager.GetUserName(User)
            };


            return View(homeCurier);

        }

        public async Task<IActionResult> Group(string? groupId)
        {
            var creater = await _context.groupCreates.Include(gc => gc.GroupCreator).FirstOrDefaultAsync(gc => gc.Id.ToString() == groupId);


            CreateGroupVM createGroupVM = new CreateGroupVM
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                GroupId = groupId,
                GroupCreator = creater != null ? creater.GroupCreator.UserName : "Null",
                GroupName = creater != null ? creater.GroupName : "Null",
            };



            var messages = await _context.groupChatDatas
                .Include(gcd => gcd.GroupMember)
                .ThenInclude(gcd => gcd.User)
                .Where(gcd => gcd.GroupMember.GroupId.ToString() == groupId)
                .Select(gcd => new GetMessagesVM
                {
                    UserId = gcd.GroupMember.UserId,
                    GroupId = gcd.GroupMember.GroupId.ToString(),
                    Message = gcd.Message,
                    UserName = gcd.GroupMember.User.UserName
                }).ToListAsync();

            GroupDataCurier curier = new GroupDataCurier
            {
                CreateGroupVM = createGroupVM,
                GetMessagesVM = messages
            };

            return View(curier);
        }

        public async Task<IActionResult> GetGroup(string? userId)
        {
            var group = await _context.groupCreates
                .Where(gc => _context.groupMembers.Any(gm => gc.Id == gm.GroupId && gm.UserId == userId ))
                .Select(gc => new GetGroupVM
                {
                    GroupId = gc.Id.ToString(),
                    GroupName = gc.GroupName,
                }).ToListAsync();


            return Json(group);
        }

        public async Task<IActionResult> GetMember(string groupId)
        {
            var member = await _context.groupMembers
                .Include(gm => gm.User)
                .Where(gm => gm.GroupId.ToString() == groupId && gm.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .Select(gm => new GetMemberVM
                {
                    UserId = gm.User.Id,
                    UserName = gm.User.UserName,
                    ProfilePhoto = _context.userProfiles.FirstOrDefault(up => up.UserId==gm.UserId).ProfilePhoto,
                    IsAdmin = _context.groupCreates.Any(gc => gc.GroupCreatorId == User.FindFirst(ClaimTypes.NameIdentifier).Value && gc.Id.ToString() == groupId)
                }).ToListAsync();

            return Json(member);
        }

        public async Task<IActionResult> ExitMember(string? userId, string groupId)
        {

            var member = await _context.groupMembers.FirstOrDefaultAsync(gm => gm.GroupId.ToString() == groupId && gm.UserId == userId);

            _context.groupMembers.Remove(member);
            await _context.SaveChangesAsync();

            return RedirectToAction("Group", "Chat");
        }

        public async Task<IActionResult> GetFriendToGroup(string groupId)
        {

            var friend = await _context.userProfiles
                .Include(up => up.User)
                .Where(up => !(_context.groupMembers.Any(gm => gm.GroupId.ToString() == groupId && gm.UserId == up.UserId )) && _context.userFriends.Any(uf => (((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId)) && uf.Status == true)))
                .Select(up => new GetUserForGroupVM
                {
                    UserId = up.UserId,
                    UserName = up.User.UserName,
                    ProfilePhoto = up.ProfilePhoto
                }).ToListAsync();

            return Json(friend);
        }

        public async Task<IActionResult> AddMember(string? userId, string? groupId)
        {
            GroupMember member = new GroupMember
            {
                UserId = userId,
                GroupId = Guid.Parse(groupId)
            };

            await _context.groupMembers.AddAsync(member);
            await _context.SaveChangesAsync();

            return RedirectToAction("Group", "Chat");
        }

        public async Task<IActionResult> ExitInGroup(string groupId)
        {

            var member = await _context.groupMembers.FirstOrDefaultAsync(gm => gm.GroupId.ToString() == groupId && gm.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);

            _context.groupMembers.Remove(member);
            await _context.SaveChangesAsync();

            var group = await _context.groupCreates.FirstOrDefaultAsync(gc => gc.Id.ToString() == groupId);

            if(group.GroupCreatorId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                var newAdmin = await _context.groupMembers.FirstOrDefaultAsync(gm => gm.GroupId.ToString() == groupId);

                if(newAdmin != null)
                {
                    group.GroupCreatorId = newAdmin.UserId;
                    await _context.SaveChangesAsync();
                }
            }



            return RedirectToAction("Group", "Chat");
        }

    }
}
