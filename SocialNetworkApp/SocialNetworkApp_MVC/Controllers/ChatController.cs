using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using System.Security.Claims;

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


            var user = await _context.userFriends.FirstOrDefaultAsync(uf => (uf.UserFollowingId == recieveId && uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value) || (uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == recieveId));

            var userprofile = user != null ? (user.UserFollowingId == recieveId ? await _context.userProfiles.Include(uf => uf.User).FirstOrDefaultAsync(up => up.UserId == user.UserFollowingId) : await _context.userProfiles.Include(uf => uf.User).FirstOrDefaultAsync(up => up.UserId == user.FriendFollowingId)) : null;

            HomeCurierFriends homeCurier = new HomeCurierFriends
            {
                userVM = users,
                recieveId = recieveId != null ? recieveId : "Null",
                FullName = userprofile != null ? userprofile.User.UserName : "Null",
                ProfilePhote = userprofile != null ? userprofile.ProfilePhoto : "Null",
                Name = userprofile != null ? userprofile.Name + " " + userprofile.Surname : "Null",
                CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                CurrentUserName = _userManager.GetUserName(User)
            };


            return View(homeCurier);

        }

        public async Task<IActionResult> MessageBox()
        {
            return View();
        }

    }
}
