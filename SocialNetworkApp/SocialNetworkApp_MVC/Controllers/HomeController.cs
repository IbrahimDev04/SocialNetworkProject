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
using SocialNetworkApp.Business.ViewModels.PostUser;
using Microsoft.SqlServer.Management.Smo;

namespace SocialNetworkApp_MVC.Controllers;
[Authorize]
public class HomeController(IWebHostEnvironment _env ,AppDbContext _context, UserManager<AppUser> _userManager, IFollowService _followService, IFriendRequestService _requestService) : Controller
{

    public async Task<IActionResult> Index(HomeCommonCurier curier)
    {

        //UserProfile userProfile = await _context.userProfiles.FirstOrDefaultAsync(up => up.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //AppUser user = await _context.appUsers
        //    .Include(au => au.UserFriendsSender)
        //    .Where(au => au.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
        //    .Select();


        //var users = await _userManager.Users.Include(up => up.UserFriendsTaker).Where(up => up.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value).ToListAsync();





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
                FollowedStatus = _context.userFriends.Any(uf => uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId && uf.Status==false),
                IsForYou = _context.userFriends.Any(uf => (((uf.UserFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.FriendFollowingId == up.UserId) || (uf.FriendFollowingId == User.FindFirst(ClaimTypes.NameIdentifier).Value && uf.UserFollowingId == up.UserId)) && uf.Status == false)),
                HaveUserStory = _context.userFriends.Any(uf => _context.userStories.Any(us => us.UserId == up.UserId)),

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
        



        HomeCurierFriends homeCurier = new HomeCurierFriends
        {
            userVM = users,
        };

        HomeCommonCurier commonCurier = new HomeCommonCurier
        {
            homeCurierFriends = homeCurier,
            getStoriesVM = userCurrentStory,
            getYourStoriesVM = storyYour,
            getPostsVM = getPosts,
        };

        if (curier.createPostVM != null)
        {
            if (curier.createPostVM.ImageUrl != null)
            {
                if (!curier.createPostVM.ImageUrl.IsValidType("image"))
                {
                    throw new InvalidTypeException("sehv");
                }
                if (!curier.createPostVM.ImageUrl.IsValidSize(2000))
                {
                    throw new InvalidSizeException("sehv size");
                }
            }
            if (curier.createPostVM.VideoUrl != null)
            {
                if (!curier.createPostVM.VideoUrl.IsValidType("video"))
                {
                    throw new InvalidTypeException("sehv");
                }
                if (!curier.createPostVM.VideoUrl.IsValidSize(2000))
                {
                    throw new InvalidSizeException("sehv size");
                }
            }

            var up = await _context.userProfiles.FirstOrDefaultAsync(up => up.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string imgFileName = curier.createPostVM.ImageUrl != null ? await curier.createPostVM.ImageUrl.FileManagedAsync(Path.Combine(_env.WebRootPath, "posts", "postContent", "imgs")) : null;
            string videoFileName = curier.createPostVM.VideoUrl != null ? await curier.createPostVM.VideoUrl.FileManagedAsync(Path.Combine(_env.WebRootPath, "posts", "postContent", "videos")) : null;
            UserPosts posts = null;
            if (curier.createPostVM.ImageUrl != null)
            {
                posts = new UserPosts
                {
                    ImageUrl = Path.Combine("posts", "postContent", "imgs", imgFileName),
                    Description = curier.createPostVM.Description != null ? curier.createPostVM.Description : null,
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    UserProfileId = up.Id,
                };
            }else if(curier.createPostVM.VideoUrl != null)
            {
                posts = new UserPosts
                {
                    VideoUrl = Path.Combine("posts", "postContent", "videos", videoFileName),
                    Description = curier.createPostVM.Description != null ? curier.createPostVM.Description : null,
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    UserProfileId = up.Id,
                };
            }else if(curier.createPostVM.VideoUrl == null && curier.createPostVM.ImageUrl == null)
            {
                posts = new UserPosts
                {
                    Description = curier.createPostVM.Description != null ? curier.createPostVM.Description : null,
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    UserProfileId = up.Id,
                };
            }
            else
            {
                throw new Exception("Error");
            }


            

            await _context.userPosts.AddAsync(posts);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");

        }


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


    public async Task<IActionResult> LikePost(string PostId)
    {

        PostStats stats = new PostStats
        {
            PostId = Guid.Parse(PostId),
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
        };

        await _context.postStats.AddAsync(stats);
        await _context.SaveChangesAsync();

        var likeCount = _context.postStats.Count(ps => ps.PostId == stats.PostId);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UnlikePost(string PostId)
    {

        var post = await _context.postStats.FirstOrDefaultAsync(ps => ps.PostId.ToString() == PostId && ps.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);

        _context.postStats.Remove(post);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");


    }

    public async Task<IActionResult> AddFavouriteList(string PostId)
    {

        PostFavorites favorites = new PostFavorites
        {
            PostId = Guid.Parse(PostId),
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
        };

        await _context.postFavorites.AddAsync(favorites);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RemoveFavouriteList(string PostId)
    {

        var post = await _context.postFavorites.FirstOrDefaultAsync(pf => pf.PostId.ToString() == PostId && pf.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);

        _context.postFavorites.Remove(post);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SendComment(string PostId, string Comment)
    {

        PostComments comments = new PostComments
        {
            UserPostsId = Guid.Parse(PostId),
            UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
            Comment = Comment
        };

        await _context.postComments.AddAsync(comments);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RemoveComment(string CommentId)
    {

        var comment = await _context.postComments.FirstOrDefaultAsync(pc => pc.Id.ToString() == CommentId && pc.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);

        _context.postComments.Remove(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Search(string searchString)
    {

        var userProfiles = _context.userProfiles.AsQueryable();
        var user = new List<SearchUserVM>();

        if (!String.IsNullOrEmpty(searchString))
        {
            user = userProfiles
                .Include(up => up.User)
                .Where(up => (up.Name.Contains(searchString) || up.Surname.Contains(searchString) || up.User.UserName.Contains(searchString)) && up.UserId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .Select(up => new SearchUserVM
                {
                    UserId = up.UserId,
                    UserName = up.User.UserName,
                    ProfilePhoto = up.ProfilePhoto
                }).ToList();
        }

        return Json(user);
    }



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
