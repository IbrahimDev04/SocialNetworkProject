namespace SocialNetworkApp.Core.Entities.Commons;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set;}
}
