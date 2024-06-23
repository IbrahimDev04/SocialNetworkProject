using SocialNetworkApp.Business.ViewModels.AppUser;
using SocialNetworkApp.Business.ViewModels.ChatData;
using SocialNetworkApp.Business.ViewModels.UserFriend;
using SocialNetworkApp.Business.ViewModels.UserStories;

namespace SocialNetworkApp.Business.ViewModels.DataCourier
{
    public class HomeCurierFriends
    {
        public List<GetUserVM> userVM {  get; set; }
        public List<GetUserFriendVM> userFriendVM { get; set; }
        public List<GetChatDataVM> sendChatDataVM { get; set; }


        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? recieveId { get; set; }
        public string? ProfilePhote {  get; set; }
        public string? CurrentUserId { get; set;}
        public string? CurrentUserName { get; set; }
    }
}
