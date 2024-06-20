using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.Business.Services.Interfaces;

public interface IEmailService
{
    public void Send(string mailTo, string subject, string body, bool isBodyHtml = false);


    public Task ConfirmeEmail(string token, string email);

    public Task ResetPassword(string token, string email, string password);

    public Task ChangePassword(AppUser user, string currentPassword, string newPassword);


    }
