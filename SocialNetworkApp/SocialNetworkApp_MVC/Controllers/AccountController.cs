using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using SocialNetworkApp.Business.Exceptions.AppUser;
using SocialNetworkApp.Business.Exceptions.Common;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Business.ViewModels.Account;
using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.DataCourier;
using SocialNetworkApp.Business.ViewModels.UserProfile;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Repositories.Interfaces;
using System.Security.Claims;

namespace SocialNetworkApp_MVC.Controllers
{
    public class AccountController(IUserService _userService, IUserProfileService _userProfileService,UserManager<AppUser> _userManager ,SignInManager<AppUser> _signInManager, IEmailService _emailService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCurier VM)
        {
            if (!ModelState.IsValid) return View(VM);

            try
            {
                AppUser user = await _userService.RegisteredAsync(VM);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var ConfirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, Email = user.Email }, Request.Scheme);

                _emailService.Send(user.Email, "Confirmation link", ConfirmationLink);
            }
            catch (RegisterFailedException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            if (!ModelState.IsValid) return View(VM);

            return RedirectToAction(nameof(SuccessfullyRegistered));
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            await _emailService.ConfirmeEmail(token, email);

            return View();
        }

        public async Task<IActionResult> SuccessfullyRegistered() 
        {
            return View();
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

                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("", "Please confirme email");
                    return View(VM);
                }

                try
                {
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
                

                

                
            }
            catch (WrongUsernameOrEmailException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            if (!ModelState.IsValid) return View(VM);

            return RedirectToAction("Index", "Home"); ;
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(FogotPasswordVM fogotPasswordVM)
        {
            if (!ModelState.IsValid) return View(fogotPasswordVM);

            var user = await _userManager.FindByEmailAsync(fogotPasswordVM.Email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { token, email = fogotPasswordVM.Email }, Request.Scheme);

            _emailService.Send(fogotPasswordVM.Email, "Reset Password", callbackUrl);


            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        public async Task<IActionResult> ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string email, ResetPasswordVM passwordVM)
        {

            if(!ModelState.IsValid) return View(passwordVM);

            await _emailService.ResetPassword(token, email, passwordVM.NewPassword);

            return RedirectToAction(nameof(SuccessfullReset));
        }

        public async Task<IActionResult> SuccessfullReset()
        {
            return View();
        }

    }
}
