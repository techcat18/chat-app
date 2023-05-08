using AutoMapper;
using ChatApplication.BLL.Models.Chat;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Functions;
using ChatApplication.DAL.Entities.Views;

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