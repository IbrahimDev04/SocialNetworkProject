namespace SocialNetworkApp.Core.Entities;

public class GroupCreate : BaseEntity
{
    public string GroupName { get; set; }

    public string GroupCreatorId {  get; set; }
    public AppUser GroupCreator { get; set; }

    public ICollection<GroupMember>? GroupMembers { get; set; }
}
