namespace SocialNetworkApp.Core.Entities;

public class ChatData : BaseEntity
{

    public string Message { get; set; }

    public string FromId { get; set; }
    public AppUser From { get; set; }


    public string ToId { get; set; }
    public AppUser To { get; set; }

}
