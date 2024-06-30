using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Business.Services.Implements;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.UserFriend;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp_MVC.Controllers;

[Authorize]
public class FriendRequestController(IFriendRequestService _requestService, UserManager<AppUser> _userManager) : Controller
{
    public async Task<IActionResult> SendRequest(string userId)
    {
        var currentUserId = _userManager.GetUserId(User);

        if (currentUserId != userId)
        {
            await _requestService.SendFollow(currentUserId, userId);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> AcceptRequest(string userId)
    {
        var currentUserId = _userManager.GetUserId(User);

        if (currentUserId != userId)
        {
            await _requestService.AcceptFollow(currentUserId, userId);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
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
