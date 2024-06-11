namespace SocialNetworkApp.Business.Exceptions.AppUser;

public class WrongUsernameOrEmailException : Exception
{
    public WrongUsernameOrEmailException() : base("Username or Password is wrong.") 
    {
    }

    public WrongUsernameOrEmailException(string? message) : base(message)
    {
    }
}
