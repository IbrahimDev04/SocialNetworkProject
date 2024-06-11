namespace SocialNetworkApp.Core.Entities;

public class UserSettings : BaseEntity
{
    public bool ProfileVisbility { get; set; }
    public bool NotifactionsMessages { get; set; }
    public bool NotifactionsComments { get; set; }

    public string? UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }


}
