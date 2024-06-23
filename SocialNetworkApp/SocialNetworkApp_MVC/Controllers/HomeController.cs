using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Business.Exceptions.Common;
using SocialNetworkApp.Business.Exceptions.UserProfile;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Business.ViewModels.UserFriend;
using SocialNetworkApp.Business.ViewModels.UserProfile;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using System.Security.Claims;
using SocialNetworkApp.Business.Extensions;
using SocialNetworkApp.Business.ViewModels.UserStories;

namespace SocialNetworkApp_MVC.Controllers;

public class HomeController(IWebHostEnvironment _env ,AppDbContext _context, UserManager<AppUser> _userManager, IFollowService _followService, IFriendRequestService _requestService) : Controller
{
    [Authorize]
    public async Task<IActionResult> Index(HomeCommonCurier curier)
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
                HaveUserStory = _context.userFriends.Any(uf => _context.userStories.Any(us => us.UserId == up.UserId))

            }).ToListAsync();

        var userCurrentStory = await _context.userStories
            .Where(us => us.CreatedTime.AddDays(1d) > DateTime.Now && _context.userFriends.Any(uf => (uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == us.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == us.UserId) && uf.Status == true))
            .Select(us => new GetStoriesVM
            {
                UserId = us.UserId,
                ImageOrVideoUrl = us.ImageOrVideoUrl,
                StatusMind = us.StatusMind,
            }).ToListAsync();

        //var userYourStory = await _context.userStories.FirstOrDefaultAsync(us => us.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value && us.CreatedTime.AddDays(1d) > DateTime.Now);

        List<GetStoriesVM> storyYour = null;
        storyYour = await _context.userStories
            .Where(us => us.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value && us.CreatedTime.AddDays(1d) > DateTime.Now)
            .Select(us => new GetStoriesVM
            {
                ImageOrVideoUrl = us.ImageOrVideoUrl,
                StatusMind = us.StatusMind,
                UserId = us.UserId,
            }).ToListAsync();


        //if (userYourStory != null)
        //{
            
                
                
        //    //    new GetStoriesVM
        //    //{
        //    //    ImageOrVideoUrl = userYourStory.ImageOrVideoUrl,
        //    //    StatusMind = userYourStory.StatusMind,
        //    //    UserId = userYourStory.UserId,
        //    //};
        //}
        

        




        HomeCurierFriends homeCurier = new HomeCurierFriends
        {
            userVM = users,
        };

        HomeCommonCurier commonCurier = new HomeCommonCurier
        {
            homeCurierFriends = homeCurier,
            getStoriesVM = userCurrentStory,
            getYourStoriesVM = storyYour,

        };

        if (curier.createStoriesVM != null)
        {
            if (curier.createStoriesVM.ImageOrVideoUrl != null)
            {
                if (!curier.createStoriesVM.ImageOrVideoUrl.IsValidType("image"))
                {
                    throw new InvalidTypeException("sehv");
                }

                if (!curier.createStoriesVM.ImageOrVideoUrl.IsValidSize(2000))
                {
                    throw new InvalidSizeException("sehv size");
                }
            }

            string fileName = await curier.createStoriesVM.ImageOrVideoUrl.FileManagedAsync(Path.Combine(_env.WebRootPath, "stories", "storyContent"));



            UserStories stories = new UserStories
            {
                ImageOrVideoUrl = Path.Combine("stories", "storyContent", fileName),
                StatusMind = curier.createStoriesVM.StatusMind != null ? curier.createStoriesVM.StatusMind : null,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                ViewCount = 0
            };

            await _context.userStories.AddAsync(stories);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }



        return View(commonCurier);

        

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
