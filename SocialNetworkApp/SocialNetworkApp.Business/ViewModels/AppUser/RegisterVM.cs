using System.ComponentModel.DataAnnotations;

namespace SocialNetworkApp.Business.ViewModels.Account;

public class RegisterVM
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Username { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string RepeatPassword { get; set; }
}
