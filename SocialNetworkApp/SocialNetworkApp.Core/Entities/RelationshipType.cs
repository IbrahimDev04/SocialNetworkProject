namespace SocialNetworkApp.Core.Entities;

public class RelationshipType : BaseEntity
{
    public string TypeName { get; set; }
    public ICollection<Relationship>? Relationships { get; set; }
}
