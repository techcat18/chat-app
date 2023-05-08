using AutoMapper;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Functions;
using ChatApplication.DAL.Entities.Views;
using ChatApplication.Shared.Models.Chat;

namespace ChatApplication.BLL.MappingProfiles;

public class ChatProfile: Profile
{
    public ChatProfile()
    {
        CreateMap<CreateChatModel, Chat>();
        CreateMap<Chat, ChatModel>();
        CreateMap<ChatView, ChatModel>();
        CreateMap<ChatFunction, ChatModel>();
    }
}