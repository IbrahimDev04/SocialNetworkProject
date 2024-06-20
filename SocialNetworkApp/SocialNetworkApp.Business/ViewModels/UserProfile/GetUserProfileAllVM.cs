using Microsoft.AspNetCore.Http;
using SocialNetworkApp.Business.Enums;

namespace SocialNetworkApp.Business.ViewModels.UserProfile;

public class GetUserProfileAllVM
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string ProfilePhoto { get; set; }
    public string Gender { get; set; }
    public string About { get; set; }
    public string CurrentWorkAt { get; set; }
    public string CurrentStudyUni { get; set; }
}
