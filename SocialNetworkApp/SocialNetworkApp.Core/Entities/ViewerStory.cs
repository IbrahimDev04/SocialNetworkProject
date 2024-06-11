namespace SocialNetworkApp.Core.Entities;

public class ViewerStory : BaseEntity
{
    public string UserId { get; set; }
    public AppUser? User { get; set; }

    public string StoryId { get; set; }
    public UserStories? Story { get; set; }
}
