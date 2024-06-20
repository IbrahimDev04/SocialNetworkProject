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

namespace SocialNetworkApp_MVC.Controllers
{
    public class ProfileController(IFriendRequestService _requestService ,AppDbContext _context,IUserService _userService , IUserProfileService _userProfileService, IEmailService _emailService, UserManager<AppUser> _userManager, IAppUserRepository _repo, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {

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

            ProfileCurier curier = new ProfileCurier
            {
                ProfileAllVM = vm,
                userVM = userVM
            };

            return View(curier);
        }

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
                else if(curier.UserProfileVM.ProfilePhoto != null)
                {
                    string fileName = await curier.UserProfileVM.ProfilePhoto.FileManagedAsync(Path.Combine(_env.WebRootPath, "imgs", "profilePhoto"));

                    await _userProfileService.UpdateUserProfilePhotoById(User.FindFirst(ClaimTypes.NameIdentifier).Value, fileName);
                }
                else if(curier.ChangableUserLocationVM != null)
                {

                }
                else
                {
                    curier = await _userProfileService.UpdateUserProfileById(curier, User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                return View(curier);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult SuccessfullChanging()
        {
            return View();
        }

        public async Task<IActionResult> UserProfile(string userId)
        {

            return View();
        }
    }
}
