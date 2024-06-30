namespace SocialNetworkApp.Business.ViewModels.PostUser;

public class GetCommentVM
{
    public string? CommentId { get; set; }
    public string? Comment {  get; set; }
    public string? PostId { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? ProfilePhoto { get; set;}
    public string? Date { get; set; }
    public bool IsYourComment { get; set;}
}
