namespace SocialNetworkApp.Business.Exceptions.AppUser;

public class SelectionOverrunException : Exception
{
    public SelectionOverrunException() : base() 
    {
    }

    public SelectionOverrunException(string? message) : base(message)
    {
    }
}
