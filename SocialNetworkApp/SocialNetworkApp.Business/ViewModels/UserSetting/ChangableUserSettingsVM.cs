namespace SocialNetworkApp.Business.ViewModels.UserSetting;

public class ChangableUserSettingsVM
{
    public bool ProfileVisbility { get; set; } = false;
    public bool NotifactionsMessages { get; set; } = false;
    public bool NotifactionsComments { get; set; } = false;
}
