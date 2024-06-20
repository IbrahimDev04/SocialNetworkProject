using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.Business.ViewModels.AppUser;

public class FogotPasswordVM
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
