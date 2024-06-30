using SocialNetworkApp.Business.ViewModels.GroupChatData;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class GroupDataCurier
{
    public CreateGroupVM CreateGroupVM { get; set; }
    public List<GetMessagesVM> GetMessagesVM { get; set; }
}
