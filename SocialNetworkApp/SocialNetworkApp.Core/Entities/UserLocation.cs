namespace SocialNetworkApp.Core.Entities;

public class UserLocation : BaseEntity
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }


    public string? UserId { get; set; }
    public UserProfile? User { get; set; }

}
