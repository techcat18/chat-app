using AutoMapper;
using ChatApplication.BLL.Models.Auth;
using ChatApplication.DAL.Entities;

namespace ChatApplication.BLL.MappingProfiles;

public class AuthProfile: Profile
{
    public AuthProfile()
    {
        CreateMap<SignupModel, User>()
            .ForMember(dest => dest.UserName, src => src.MapFrom(opt => opt.Email));
    }
}