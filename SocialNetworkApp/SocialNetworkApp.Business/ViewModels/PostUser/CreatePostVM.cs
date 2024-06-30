using Microsoft.AspNetCore.Http;

namespace SocialNetworkApp.Business.ViewModels.PostUser;

public class CreatePostVM
{
    public IFormFile? ImageUrl { get; set; }
    public IFormFile? VideoUrl { get; set; }

    public string? Description { get; set; }
    public string? UserId { get; set; }
}
