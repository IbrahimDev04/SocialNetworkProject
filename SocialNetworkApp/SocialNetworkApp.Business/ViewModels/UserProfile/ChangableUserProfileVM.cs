using Microsoft.AspNetCore.Http;
using SocialNetworkApp.Business.Enums;

namespace SocialNetworkApp.Business.ViewModels.UserProfile;

public class ChangableUserProfileVM
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
    public GenderEnum Gender { get; set; }
    public string? About { get; set; }
    public string? CurrentWorkAt { get; set; }
    public string? CurrentStudyUni { get; set; }
}
