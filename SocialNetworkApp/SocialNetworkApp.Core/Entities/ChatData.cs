namespace SocialNetworkApp.Core.Entities;

public class ChatData : BaseEntity
{

    public string Message { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }
}
