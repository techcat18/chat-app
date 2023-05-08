using AutoMapper;
using ChatApplication.DAL.Entities;
using ChatApplication.Shared.Models.Auth;

namespace ChatApplication.BLL.MappingProfiles;

public class AuthProfile: Profile
{
    public AuthProfile()
    {
        CreateMap<SignupModel, User>()
            .ForMember(dest => dest.UserName, src => src.MapFrom(opt => opt.Email));
    }
}