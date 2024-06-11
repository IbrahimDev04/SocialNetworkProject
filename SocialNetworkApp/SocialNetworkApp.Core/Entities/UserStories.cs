namespace SocialNetworkApp.Core.Entities;

public class UserStories : BaseEntity
{
    public string ImageOrVideoUrl { get; set; }
    public int ViewCount { get; set; } 

    public string UserId { get; set; }
    public AppUser? User { get; set; }

    public ICollection<ViewerStory>? Stories { get; set; }
}
