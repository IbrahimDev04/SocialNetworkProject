namespace SocialNetworkApp.Business.Exceptions.Common;

public class NotFountException : Exception
{
    public NotFountException() : base()
    {
    }

    public NotFountException(string? message) : base(message)
    {
    }
}
