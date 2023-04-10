using AutoMapper;
using ChatApplication.BLL.Models.GroupChat;
using ChatApplication.DAL.Entities;

namespace ChatApplication.BLL.MappingProfiles;

public class GroupChatProfile: Profile
{
    public GroupChatProfile()
    {
        CreateMap<CreateGroupChatModel, GroupChat>();
        CreateMap<GroupChat, GroupChatModel>();
    }
}