using AutoMapper;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Functions;
using ChatApplication.Shared.Models.Message;

namespace ChatApplication.BLL.MappingProfiles;

public class MessageProfile: Profile
{
    public MessageProfile()
    {
        CreateMap<Message, MessageModel>()
            .ForMember(dest => dest.SenderEmail, src => src.MapFrom(opt => opt.Sender.Email));

        CreateMap<MessageFunction, MessageModel>();
        
        CreateMap<CreateMessageModel, Message>();
    }
}