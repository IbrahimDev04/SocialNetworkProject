using Microsoft.AspNetCore.Http;

namespace SocialNetworkApp.Business.ViewModels.UserStories;

public class CreateStoriesVM
{
    public IFormFile? ImageOrVideoUrl { get; set; }

    public string? StatusMind { get; set; }

    public int ViewCount { get; set; }

    public string UserId { get; set; }

}
