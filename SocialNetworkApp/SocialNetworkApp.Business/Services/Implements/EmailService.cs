//using Azure.Core;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json.Linq;
using SocialNetworkApp.Business.Exceptions.Common;
using SocialNetworkApp.Business.Services.Interfaces;
using SocialNetworkApp.Core.Entities;
using SocialNetworkApp.DAL.Repositories.Interfaces;
using System.Net;
using System.Net.Mail;

namespace SocialNetworkApp.Business.Services.Implements;

public class EmailService(IConfiguration _configuration, UserManager<AppUser> _userManager, IAppUserRepository _repo) : IEmailService
{


    public async Task ConfirmeEmail(string token, string email)
    {

        AppUser user = await _userManager.FindByEmailAsync(email);

        if (user == null) throw new NotFountException();

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded) throw new NotFountException();


    }

    public async Task ChangePassword(AppUser user, string currentPassword,string newPassword)
    {

        if (user == null) throw new NotFountException();

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!result.Succeeded) throw new NotFountException();


    }





    public void Send(string mailTo, string subject, string body, bool isBodyHtml = false)
    {
        SmtpClient smtpClient = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]));

        smtpClient.EnableSsl = true;

        smtpClient.Credentials = new NetworkCredential(_configuration["Email:Login"], _configuration["Email:Password"]);

        MailAddress from = new MailAddress(_configuration["Email:Login"], "Loomy");
        MailAddress to = new MailAddress(mailTo);

        MailMessage message = new MailMessage(from, to);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = isBodyHtml;

        smtpClient.Send(message);
    }

    public async Task ResetPassword(string token, string email, string password)
    {
        AppUser user = await _userManager.FindByEmailAsync(email);

        if (user == null) throw new NotFountException();

        var result = await _userManager.ResetPasswordAsync(user, token, password);

        if (!result.Succeeded) throw new NotFountException();
    }
}
