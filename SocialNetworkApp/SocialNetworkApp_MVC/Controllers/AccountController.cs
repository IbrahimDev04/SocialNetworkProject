using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Business.Exceptions.AppUser;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.Account;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Business.ViewModels.UserProfile;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Repositories.Interfaces;
using System.Security.Claims;

namespace SocialNetworkApp_MVC.Controllers
{
    public class AccountController(IUserService _userService, IUserProfileService _userProfileService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCurier VM)
        {
            //if (!ModelState.IsValid) return View(VM.RegisterVM);

            IdentityResult result = await _userService.RegisteredAsync(VM);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description); 
                }
                return View(VM.RegisterVM);
            }


            return RedirectToAction("Login" ,"Account");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM VM)
        {
            if (!ModelState.IsValid) return View(VM);

            
            try
            {
                AppUser user = await _userService.IsExistsUsernameOrEmail(VM.UserNameOrEmail);

                if (!ModelState.IsValid) return View(VM);

                var result = await _userService.LoginInAsync(VM, user);

                if (result.IsLockedOut)
                {
                    try
                    {
                        await _userService.CheckSelectionOverrun(user);
                    }
                    catch (SelectionOverrunException ex) 
                    {
                        ModelState.AddModelError("", ex.Message);
                        return View(VM);
                    }
                }
            }
            catch (WrongUsernameOrEmailException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            if (!ModelState.IsValid) return View(VM);

            return RedirectToAction("Index", "Home"); ;
        }

    }
}
