namespace SocialNetworkApp.Business.Exceptions.UserProfile;

public class InvalidSizeException : Exception
{
    public InvalidSizeException() : base("Size Error")
    {
    }

    public InvalidSizeException(string? message) : base(message)
    {
    }
}
