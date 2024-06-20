namespace SocialNetworkApp.Business.Exceptions.UserProfile;

public class InvalidTypeException : Exception
{
    public InvalidTypeException() : base("Type Error") 
    {
    }

    public InvalidTypeException(string? message) : base(message)
    {
    }
}
