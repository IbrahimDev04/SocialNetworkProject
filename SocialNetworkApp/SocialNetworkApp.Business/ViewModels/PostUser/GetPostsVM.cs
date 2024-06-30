namespace SocialNetworkApp.Business.ViewModels.PostUser;

public class GetPostsVM
{
    public string? Date { get; set; }
    public string? PostId { get; set; }
    public string UserName { get; set; }
    public string ProfilePhoto { get; set; }
    public string Description { get; set;}
    public string ImageUrl { get; set; }
    public string VideoUrl { get; set; }
    public string UserId { get; set; }
    public int? LikeCount { get; set; }
    public bool HaveLikePost { get; set; }
    public bool HaveFavouritePost { get; set; }
    public List<GetCommentVM>? GetCommentVM { get; set; }
}
