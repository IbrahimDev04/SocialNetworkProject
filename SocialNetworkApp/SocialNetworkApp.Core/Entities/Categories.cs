namespace SocialNetworkApp.Core.Entities;

public class Categories : BaseEntity
{
    public string CategoryName { get; set; }
    public ICollection<UserPosts>? UserPosts { get; set; }
}
