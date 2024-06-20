using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Business.ViewModels.UserProfile;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Contexts;
using System.Security.Claims;

namespace SocialNetworkApp_MVC.ViewComponents
{
    public class HeaderViewComponent(AppDbContext _context, IHttpContextAccessor _httpContextAccessor) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            UserProfile userProfile = await _context.userProfiles.FirstOrDefaultAsync(up => up.UserId == _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            AppUser user = await _context.appUsers.SingleOrDefaultAsync(u => u.Id == _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);


            GetUserProfileVM profileVM = new GetUserProfileVM
            {
                Name = userProfile.Name,
                Surname = userProfile.Surname,
                ProfilePhoto = userProfile.ProfilePhoto
            };


            GetUserVM userVM = new GetUserVM
            {
                UserName = user.UserName,
            };
            

            HomeCurier homeCurier = new HomeCurier
            {
                userProfileVM = profileVM,
                userVM = userVM,
            };

            return View(homeCurier);
        }
    }
}
