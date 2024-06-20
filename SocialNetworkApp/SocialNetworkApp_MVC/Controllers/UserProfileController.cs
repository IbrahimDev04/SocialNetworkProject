using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Business.Services.Implements;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.UserFriend;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp_MVC.Controllers
{
    public class UserProfileController(UserManager<AppUser> _userManager, IFriendRequestService _requestService) : Controller
    {
        //public async Task<IActionResult> Profile(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    var currentUserId = _userManager.GetUserId(User);
        //    var isFriend = await _requestService.AreFriendsAsync(currentUserId, userId);

        //    var model = new UserProfileVM
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName,
        //        IsFriend = isFriend
        //    };

        //    return View(model);
        //}

        //public async Task<IActionResult> PendingRequests()
        //{
        //    var currentUserId = _userManager.GetUserId(User);
        //    var pendingRequests = await _requestService.GetPendingFriendRequestsAsync(currentUserId);

        //    return View(pendingRequests);
        //}
    }
}
