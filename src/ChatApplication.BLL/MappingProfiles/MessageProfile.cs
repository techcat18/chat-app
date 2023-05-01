using AutoMapper;
using ChatApplication.BLL.Models.Message;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Functions.Results;

namespace ChatApplication.BLL.MappingProfiles;

public class MessageProfile: Profile
{
    public MessageProfile()
    {
        CreateMap<Message, MessageModel>()
            .ForMember(dest => dest.SenderEmail, src => src.MapFrom(opt => opt.Sender.Email));

        CreateMap<MessageFuncResult, MessageModel>();
        
        CreateMap<CreateMessageModel, Message>();
    }
}