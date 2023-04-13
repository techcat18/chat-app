using AutoMapper;
using ChatApplication.BLL.Models.Chat;
using ChatApplication.DAL.Entities;

namespace ChatApplication.BLL.MappingProfiles;

public class ChatProfile: Profile
{
    public ChatProfile()
    {
        CreateMap<CreateChatModel, Chat>();
        CreateMap<Chat, ChatModel>();
    }
}