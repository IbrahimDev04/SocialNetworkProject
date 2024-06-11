namespace SocialNetworkApp.Core.Entities;

public class PostTags : BaseEntity
{
    public string TagName { get; set; }

    public string PostId {  get; set; }
    public UserPosts? Post { get; set; }
}
