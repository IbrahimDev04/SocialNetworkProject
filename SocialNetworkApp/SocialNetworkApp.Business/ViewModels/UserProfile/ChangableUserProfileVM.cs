namespace SocialNetworkApp.Business.ViewModels.UserProfile;

public class ChangableUserProfileVM
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Gender { get; set; }
    public string About { get; set; }
    public string CurrentWorkAt { get; set; }
    public string CurrentStudyUni { get; set; }
}
