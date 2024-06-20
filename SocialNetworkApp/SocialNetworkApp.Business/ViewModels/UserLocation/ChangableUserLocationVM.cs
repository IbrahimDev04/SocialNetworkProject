namespace SocialNetworkApp.Business.ViewModels.UserLocation;

public class ChangableUserLocationVM
{
    public string? UserId { get; set; }
    public string? Country { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? State { get; set; } = string.Empty;
}
