using SocialNetworkApp.Business.ViewModels.UserFriend;
using SocialNetworkApp.Core.Entities;

namespace SocialNetworkApp.Business.Services.Interfaces;

public interface IFriendRequestService
{
    public Task SendFollow(string currentUserId, string userIdToFollow);
    public Task AcceptFollow(string currentUserId, string userIdToFollow);
    public Task UnfollowUserAsync(string currentUserId, string userIdToUnfollow);
    public Task<List<UserFriendRequestVM>> GetUserSender(string currentUserId);




    }
