namespace SocialNetworkApp.Business.Exceptions.AppUser;

public class RegisterFailedException : Exception
{
    public RegisterFailedException() : base("")
    {
    }

    public RegisterFailedException(string? message) : base(message)
    {
    }
}
