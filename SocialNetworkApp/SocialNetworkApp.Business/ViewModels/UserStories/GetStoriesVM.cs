using Microsoft.AspNetCore.Http;

namespace SocialNetworkApp.Business.ViewModels.UserStories;

public class GetStoriesVM
{
    public string? ImageOrVideoUrl { get; set; }

    public string? StatusMind { get; set; }


    public string? UserId { get; set; }
}
