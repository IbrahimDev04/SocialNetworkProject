namespace SocialNetworkApp.Core.Entities;

public class GroupMember : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }

    public Guid GroupId { get; set; }
    public GroupCreate Group { get; set; }

    public ICollection<GroupChatData> GroupChatDatas { get; set;}
}
