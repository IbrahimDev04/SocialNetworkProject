namespace SocialNetworkApp.Business.ViewModels.AppUser;

public class GetUserVM
{
    public string? Id { get; set; }

    public string? UserName { get; set; }
    
    public string? Name { get; set; }

    public string? Surname { get; set;}

    public string? ProfilePhoto { get; set; }

    public bool Status { get; set; }

    public bool FollowedStatus { get; set; }

    public bool IsForYou { get; set; }

    public int? Count { get; set; }

}
