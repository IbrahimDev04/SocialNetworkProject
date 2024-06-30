namespace SocialNetworkApp.Core.Entities;

public class GroupChatData : BaseEntity
{
    public string Message { get; set; }

    public Guid GroupMemberId { get; set; }
    public GroupMember GroupMember { get; set; }

}
