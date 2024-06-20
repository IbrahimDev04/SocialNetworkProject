using Microsoft.AspNetCore.Http;

namespace SocialNetworkApp.Business.ViewModels.UserProfile;

public class GetUserProfileVM
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string ProfilePhoto { get; set; }
}
