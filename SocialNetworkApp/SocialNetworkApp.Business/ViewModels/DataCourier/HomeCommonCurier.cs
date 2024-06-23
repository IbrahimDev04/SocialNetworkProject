using SocialNetworkApp.Business.ViewModels.UserStories;

namespace SocialNetworkApp.Business.ViewModels.DataCourier;

public class HomeCommonCurier
{
    public HomeCurierFriends? homeCurierFriends { get; set; }
    public CreateStoriesVM? createStoriesVM { get; set; }
    public List<GetStoriesVM>? getStoriesVM { get; set; }  
    public List<GetStoriesVM>? getYourStoriesVM { get; set; }
}
