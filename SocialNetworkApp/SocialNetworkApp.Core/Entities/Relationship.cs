namespace SocialNetworkApp.Core.Entities;

public class Relationship : BaseEntity
{
    public string UserId { get; set; }
    public AppUser? User { get; set; }

    public string RelationalUserId { get; set; }
    public AppUser? RelationalUser { get; set; }

    public string RelationshipTypeId { get; set; }
    public RelationshipType? RelationshipType { get; set; }

}
