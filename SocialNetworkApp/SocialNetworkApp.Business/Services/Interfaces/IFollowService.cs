namespace SocialNetworkApp.Business.Services.Interfaces;

public interface IFollowService
{
    public Task FollowUserAsync(string currentUserId, string userIdToFollow);

    public Task UnfollowUserAsync(string currentUserId, string userIdToUnfollow);
}
