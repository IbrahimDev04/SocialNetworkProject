using Microsoft.AspNetCore.Identity;

namespace SocialNetworkApp.Core.Entities;

public class AppUser : IdentityUser
{
    public ICollection<UserProfile>? UserProfiles { get; set; }
    public ICollection<Relationship> RelationshipsUser { get; set; }
    public ICollection<Relationship> RelationshipsRelUser { get; set; }
    public ICollection<UserFriends>? UserFriendsSender { get; set; }
    public ICollection<UserFriends>? UserFriendsTaker { get; set; }
    public ICollection<UserStories>? UserStories { get; set; }
    public ICollection<ViewerStory>? Stories { get; set; }
    public ICollection<UserPosts>? UserPosts { get; set; }
    public ICollection<PostFavorites>? PostFavorites { get; set; }
    public ICollection<PostComments>? PostComments { get; set; }
    public ICollection<ChatData>? ChatDatas { get; set; }
}
