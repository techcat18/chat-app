using AutoMapper;
using ChatApplication.BLL.Models.Chat;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Functions.Results;
using ChatApplication.DAL.Views;

namespace ChatApplication.BLL.MappingProfiles;

public class ChatProfile: Profile
{
    public ChatProfile()
    {
        CreateMap<CreateChatModel, Chat>();
        CreateMap<Chat, ChatModel>();
        CreateMap<ChatView, ChatModel>();
        CreateMap<ChatFuncResult, ChatModel>();
    }
}