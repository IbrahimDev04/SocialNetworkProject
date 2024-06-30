using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SocialNetworkApp.Business.Enums;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Business.Extensions;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using SocialNetworkApp.DAL.Repositories.Interfaces;
using System.Security.Claims;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.UserProfile;
using Microsoft.SqlServer.Management.Smo;
using SocialNetworkApp.Business.ViewModels.UserFriend;
using SocialNetworkApp.Business.Services.Implements;
using SocialNetworkApp.Business.ViewModels.PostUser;
using Microsoft.AspNetCore.Authorization;

namespace SocialNetworkApp_MVC.Controllers;

public class ProfileController(IFriendRequestService _requestService ,AppDbContext _context,IUserService _userService , IUserProfileService _userProfileService, IEmailService _emailService, UserManager<AppUser> _userManager, IAppUserRepository _repo, IWebHostEnvironment _env) : Controller
{
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userProfileData = await _context.userProfiles
            .Include(up => up.User)
            .Where(up => up.User.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
            .Select(up => new GetAnotherUserProfileVM
            {
                ProfilePhoto = up.ProfilePhoto,
                FullName = up.Name + " " + up.Surname,
                UserName = up.User.UserName,
                AtStudy = up.CurrentStudyUni,
                AtWork = up.CurrentWorkAt,
                Location = _context.userLocations.FirstOrDefault(ul => ul.UserId == up.UserId).Country,
                About = up.About
            }).ToListAsync();

        UserProfile userProfile = await _context.userProfiles.FirstOrDefaultAsync(up => up.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);

        AppUser user = await _context.appUsers.SingleOrDefaultAsync(u => u.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value);

        GetUserVM userVM = new GetUserVM
        {
            UserName = user.UserName,
        };


        GetUserProfileAllVM vm = new GetUserProfileAllVM
        {
            Name = userProfile.Name,
            Surname = userProfile.Surname,
            About = userProfile.About,
            CurrentStudyUni = userProfile.CurrentStudyUni,
            CurrentWorkAt = userProfile.CurrentWorkAt,
            Gender = userProfile.Gender,
            ProfilePhoto = userProfile.ProfilePhoto
        };

        var users = await _context.userProfiles
            .Take(6)
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
                FollowedStatus = _context.userFriends.Any(uf => uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId && uf.Status == false),
                IsForYou = _context.userFriends.Any(uf => (((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId)) && uf.Status == false)),
                HaveUserStory = _context.userFriends.Any(uf => _context.userStories.Any(us => us.UserId == up.UserId)),

            }).ToListAsync();
        var getPosts = await _context.userPosts
            .Include(up => up.User)
            .Include(up => up.UserProfile)
            .Where(up => _context.userFriends.Any(uf => ((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId) && uf.Status == true)) || up.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
            .Select(up => new GetPostsVM
            {
                Date = GetTimeDifference(up.CreatedTime),
                PostId = up.Id.ToString(),
                UserId = up.UserId,
                Description = up.Description,
                ImageUrl = up.ImageUrl,
                VideoUrl = up.VideoUrl,
                UserName = up.User.UserName,
                ProfilePhoto = up.UserProfile.ProfilePhoto,
                LikeCount = _context.postStats.Where(ps => up.Id == ps.PostId).ToList().Count,
                HaveLikePost = _context.postStats.Any(ps => up.Id == ps.PostId && up.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value),
                HaveFavouritePost = _context.postFavorites.Any(pf => up.Id == pf.PostId && up.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value),
                GetCommentVM = _context.postComments
                    .Include(up => up.User)
                    .Where(pc => pc.UserPostsId == up.Id)
                    .Select(up => new GetCommentVM
                    {
                        IsYourComment = up.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        CommentId = up.Id.ToString(),
                        Comment = up.Comment,
                        PostId = up.UserPostsId.ToString(),
                        UserId = up.UserId,
                        UserName = up.User.UserName,
                        ProfilePhoto = _context.userProfiles.FirstOrDefault(ud => ud.UserId == up.UserId).ProfilePhoto,
                        Date = GetTimeDifference(up.CreatedTime)
                    }).ToList()
            }).ToListAsync();


        ProfileCurier curier = new ProfileCurier
        {
            GetAnotherUserProfileVM = userProfileData,
            ProfileAllVM = vm,
            userVM = userVM,
            GetUserVM = users,
            getPostsVM = getPosts,
            FriendCount = _context.userProfiles.Where(up => (up.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value) && _context.userFriends.Any(uf => ((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId)))).ToList().Count,
        };

        return View(curier);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ProfileSettings()
    {
        if(User.Identity.IsAuthenticated)
        {
            SettingsCurier curier = await _userProfileService.GetUserProfileById(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return View(curier);
        }
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> GetFriend()
    {


        var userProfileData = await _context.userProfiles
            .Include(up => up.User)
            .Where(up => up.User.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value)
            .Select(up => new GetAnotherUserProfileVM
            {
                ProfilePhoto = up.ProfilePhoto,
                FullName = up.Name + " " + up.Surname,
                UserName = up.User.UserName,
                AtStudy = up.CurrentStudyUni,
                AtWork = up.CurrentWorkAt,
                Location = _context.userLocations.FirstOrDefault(ul => ul.UserId == up.UserId).Country,
                About = up.About
            }).ToListAsync();


        var users = await _context.userProfiles
            .Include(up => up.User)
            .Where(up => up.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value && _context.userFriends.Any(uf => ((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId))))
            .Select(up => new GetUserVM
            {
                Id = up.UserId,
                UserName = up.User.UserName,
                Name = up.Name,
                Surname = up.Surname,
                ProfilePhoto = up.ProfilePhoto,
                Status = _context.userFriends.Any(uf => ((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId))),
                FollowedStatus = _context.userFriends.Any(uf => uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId && uf.Status == false),
                IsForYou = _context.userFriends.Any(uf => (((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId)) && uf.Status == false)),
                HaveUserStory = _context.userFriends.Any(uf => _context.userStories.Any(us => us.UserId == up.UserId)),

            }).ToListAsync();


        var curier = new FriendCurier
        {
            GetAnotherUserProfileVM = userProfileData,
            GetUserVM = users
        };

        return View(curier);
    }

    [Authorize]
    public async Task<IActionResult> GetAnotherFriend(string userId)
    {


        var userProfileData = await _context.userProfiles
            .Include(up => up.User)
            .Where(up => up.User.Id == userId)
            .Select(up => new GetAnotherUserProfileVM
            {
                UserId = userId,
                ProfilePhoto = up.ProfilePhoto,
                FullName = up.Name + " " + up.Surname,
                UserName = up.User.UserName,
                AtStudy = up.CurrentStudyUni,
                AtWork = up.CurrentWorkAt,
                Location = _context.userLocations.FirstOrDefault(ul => ul.UserId == up.UserId).Country,
                About = up.About
            }).ToListAsync();


        var users = await _context.userProfiles
            .Include(up => up.User)
            .Where(up => up.UserId != userId && _context.userFriends.Any(uf => ((uf.UserFollowingId == userId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == userId && uf.UserFollowingId == up.UserId))))
            .Select(up => new GetUserVM
            {
                Id = up.UserId,
                UserName = up.User.UserName,
                Name = up.Name,
                Surname = up.Surname,
                ProfilePhoto = up.ProfilePhoto,
                Status = _context.userFriends.Any(uf => ((uf.UserFollowingId == userId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == userId && uf.UserFollowingId == up.UserId))),
                FollowedStatus = _context.userFriends.Any(uf => uf.UserFollowingId == userId && uf.FriendFollowingId == up.UserId && uf.Status == false),
                IsForYou = _context.userFriends.Any(uf => (((uf.UserFollowingId == userId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == userId && uf.UserFollowingId == up.UserId)) && uf.Status == false)),
                HaveUserStory = _context.userFriends.Any(uf => _context.userStories.Any(us => us.UserId == up.UserId)),

            }).ToListAsync();


        var curier = new FriendCurier
        {
            GetAnotherUserProfileVM = userProfileData,
            GetUserVM = users
        };

        return View(curier);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ProfileSettings(SettingsCurier curier)
    {
        if(!ModelState.IsValid) return View(curier);

        if (User.Identity.IsAuthenticated)
        {

            if(curier.ChangePasswordVM != null)
            {
                AppUser user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var result = await _userManager.ChangePasswordAsync(user, curier.ChangePasswordVM.CurrentPassword, curier.ChangePasswordVM.NewPassword);


                _emailService.Send(user.Email, "Confirmation link", "Your account password successfully changed.");

                return RedirectToAction(nameof(SuccessfullChanging));
            }
            else if(curier.UserProfileVM != null)
            {
                if(curier.UserProfileVM.ProfilePhoto != null)
                {
                    string fileName = await curier.UserProfileVM.ProfilePhoto.FileManagedAsync(Path.Combine(_env.WebRootPath, "imgs", "profilePhoto"));

                    await _userProfileService.UpdateUserProfilePhotoById(User.FindFirst(ClaimTypes.NameIdentifier).Value, fileName);
                }
                else
                {
                    await _userProfileService.UpdateUserProfileById(curier,User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }

            }
            else if(curier.ChangableUserLocationVM != null)
            {
                await _userProfileService.UpdateUserLocationById(curier, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                curier = await _userProfileService.UpdateUserProfileById(curier, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            return RedirectToAction("Index", "Profile");
        }

        return RedirectToAction("Index", "Home");
    }
    [Authorize]
    public IActionResult SuccessfullChanging()
    {
        return View();
    }
    [Authorize]
    public async Task<IActionResult> UserProfile(string userId)
    {

        return View();
    }


    public async Task<IActionResult> AnotherUserProfile(string userId)
    {
        if(userId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
        {
            return RedirectToAction("Index");
        }

        var userProfileData = await _context.userProfiles
            .Include(up => up.User)
            .Where(up => up.User.Id == userId)
            .Select(up => new GetAnotherUserProfileVM
            {
                UserId = userId,
                ProfilePhoto = up.ProfilePhoto,
                FullName = up.Name + " " + up.Surname,
                UserName = up.User.UserName,
                AtStudy = up.CurrentStudyUni,
                AtWork = up.CurrentWorkAt,
                Location = _context.userLocations.FirstOrDefault(ul => ul.UserId == up.UserId).Country,
                About = up.About
            }).ToListAsync();


        //var userFriendData = await _context.userFriends
        //    .Take(6)
        //    .Include(uf => uf.FriendFollowing)
        //    .Where(uf => uf.UserFollowingId == userId)
        //    .Select(uf => new GetAnotherUserFriendVM
        //    {
        //        UserId = uf.FriendFollowingId,
        //        UserName = uf.FriendFollowing.UserName,
        //        ProfilePhoto = _context.userProfiles.FirstOrDefault(up => up.UserId == uf.FriendFollowingId).ProfilePhoto
        //    }).ToListAsync();


        //if(userFriendData == null)
        //{
        //    userFriendData = await _context.userFriends
        //        .Take(6)
        //        .Include(uf => uf.UserFollowing)
        //        .Where(uf => uf.FriendFollowingId == userId)
        //        .Select(uf => new GetAnotherUserFriendVM
        //        {
        //            UserId = uf.UserFollowingId,
        //            UserName = uf.UserFollowing.UserName,
        //            ProfilePhoto = _context.userProfiles.FirstOrDefault(up => up.UserId == uf.UserFollowingId).ProfilePhoto
        //        }).ToListAsync();
        //}

        var users = await _context.userProfiles
            .Take(6)
            .Include(up => up.User)
            .Where(up => up.UserId != userId)
            .Select(up => new GetUserVM
            {
                Id = up.UserId,
                UserName = up.User.UserName,
                Name = up.Name,
                Surname = up.Surname,
                ProfilePhoto = up.ProfilePhoto,
                Status = _context.userFriends.Any(uf => ((uf.UserFollowingId == userId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == userId && uf.UserFollowingId == up.UserId))),
                FollowedStatus = _context.userFriends.Any(uf => uf.UserFollowingId == userId && uf.FriendFollowingId == up.UserId && uf.Status == false),
                IsForYou = _context.userFriends.Any(uf => (((uf.UserFollowingId == userId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == userId && uf.UserFollowingId == up.UserId)) && uf.Status == false)),
                HaveUserStory = _context.userFriends.Any(uf => _context.userStories.Any(us => us.UserId == up.UserId)),

            }).ToListAsync();


        var getPosts = await _context.userPosts
            .Include(up => up.User)
            .Include(up => up.UserProfile)
            .Where(up => _context.userFriends.Any(uf => ((uf.UserFollowingId == userId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == userId && uf.UserFollowingId == up.UserId) && uf.Status == true)) || up.UserId == userId)
            .Select(up => new GetPostsVM
            {
                Date = GetTimeDifference(up.CreatedTime),
                PostId = up.Id.ToString(),
                UserId = up.UserId,
                Description = up.Description,
                ImageUrl = up.ImageUrl,
                VideoUrl = up.VideoUrl,
                UserName = up.User.UserName,
                ProfilePhoto = up.UserProfile.ProfilePhoto,
                LikeCount = _context.postStats.Where(ps => up.Id == ps.PostId).ToList().Count,
                HaveLikePost = _context.postStats.Any(ps => up.Id == ps.PostId && up.UserId == userId),
                HaveFavouritePost = _context.postFavorites.Any(pf => up.Id == pf.PostId && up.UserId == userId),
                GetCommentVM = _context.postComments
                    .Include(up => up.User)
                    .Where(pc => pc.UserPostsId == up.Id)
                    .Select(up => new GetCommentVM
                    {
                        IsYourComment = up.UserId == userId,
                        CommentId = up.Id.ToString(),
                        Comment = up.Comment,
                        PostId = up.UserPostsId.ToString(),
                        UserId = up.UserId,
                        UserName = up.User.UserName,
                        ProfilePhoto = _context.userProfiles.FirstOrDefault(ud => ud.UserId == up.UserId).ProfilePhoto,
                        Date = GetTimeDifference(up.CreatedTime)
                    }).ToList()
            }).ToListAsync();




        var curier = new AnotherUserCurier
        {
            UserId = userId, 
            Status = _context.userFriends.Any(uf => ((uf.UserFollowingId == userId && uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value) || (uf.FriendFollowingId == userId && uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value))),
            FollowedStatus = _context.userFriends.Any(uf => uf.UserFollowingId == userId && uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.Status == false),
            IsForYou = _context.userFriends.Any(uf => (((uf.UserFollowingId == userId && uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value) || (uf.FriendFollowingId == userId && uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value)) && uf.Status == false)),
            GetAnotherUserProfileVM = userProfileData,
            getPostsVM = getPosts,
            GetUserVM = users,
            FriendCount = _context.userProfiles.Where(up => (up.UserId != userId) && _context.userFriends.Any(uf => ((uf.UserFollowingId == userId && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == userId && uf.UserFollowingId == up.UserId)))).ToList().Count,
        };

        return View(curier);
    }
    [Authorize]
    public static string GetTimeDifference(DateTime date)
    {
        DateTime now = DateTime.Now;
        TimeSpan timeDifference = now - date;

        if (timeDifference.TotalDays < 1)
        {
            int hours = (int)timeDifference.TotalHours;
            return $"{hours}h";
        }
        else if (timeDifference.TotalDays < 30)
        {
            int days = (int)timeDifference.TotalDays;
            return $"{days}d";
        }
        else
        {
            int months = (int)(timeDifference.TotalDays / 30);
            return $"{months}m";
        }
    }
}
