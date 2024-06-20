using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.Business.ViewModels.AppUser;

public class ResetPasswordVM
{
    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required, DataType(DataType.Password), Compare(nameof(NewPassword))]
    public string RepeatPassword { get; set; }
}
