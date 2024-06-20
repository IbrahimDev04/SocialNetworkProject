using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Business.Exceptions.Common;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Business.ViewModels.UserFriend;
using SocialNetworkApp.Business.ViewModels.UserProfile;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using System.Security.Claims;

namespace SocialNetworkApp_MVC.Controllers;

public class HomeController(AppDbContext _context, UserManager<AppUser> _userManager, IFollowService _followService, IFriendRequestService _requestService) : Controller
{
    [Authorize]
    public async Task<IActionResult> Index()
    {

        //UserProfile userProfile = await _context.userProfiles.FirstOrDefaultAsync(up => up.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //AppUser user = await _context.appUsers
        //    .Include(au => au.UserFriendsSender)
        //    .Where(au => au.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
        //    .Select();


        //var users = await _userManager.Users.Include(up => up.UserFriendsTaker).Where(up => up.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value).ToListAsync();





        var users = await _context.userProfiles
            .Include(up => up.User)
            .Where(up => up.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            .Select(up => new GetUserVM
            {
                Id = up.UserId,
                UserName = up.User.UserName,
                Name = up.Name,
                Surname = up.Surname,
                ProfilePhoto = up.ProfilePhoto,
                Status = _context.userFriends.Any(uf => ((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId))),
                FollowedStatus = _context.userFriends.Any(uf => uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId && uf.Status==false),
                IsForYou = _context.userFriends.Any(uf => (((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId)) && uf.Status == false)),
                
            }).ToListAsync();

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        
        

        HomeCurierFriends homeCurier = new HomeCurierFriends
        {
            userVM = users,
        };


        return View(homeCurier);

        

        //var users = await _userManager.Users
        //        .Where(u => u.Id != _userManager.GetUserId(User))
        //        .ToListAsync();

        //return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> Follow(string userId)
    {
        var currentUserId = _userManager.GetUserId(User);

        if (currentUserId != userId)
        {
            await _followService.FollowUserAsync(currentUserId, userId);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Unfollow(string userId)
    {
        var currentUserId = _userManager.GetUserId(User);

        if (currentUserId != userId)
        {
            await _followService.UnfollowUserAsync(currentUserId, userId);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> SendRequest(string userId)
    {
        var currentUserId = _userManager.GetUserId(User);

        if (currentUserId != userId)
        {
            await _requestService.SendFollow(currentUserId, userId);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> AcceptRequest(string userId)
    {
        var currentUserId = _userManager.GetUserId(User);

        if (currentUserId != userId)
        {
            await _requestService.AcceptFollow(userId, currentUserId);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> UnfollowUser(string userId)
    {
        var currentUserId = _userManager.GetUserId(User);

        if (currentUserId != userId)
        {
            await _requestService.UnfollowUserAsync(currentUserId, userId);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> FriendRequests()
    {

        var currentUserId = _userManager.GetUserId(User);


        var users = await _requestService.GetUserSender(currentUserId);

        return View(users);
    }
}
