using AutoMapper;
using ChatApplication.BLL.Models.User;
using ChatApplication.DAL.Entities;

namespace ChatApplication.BLL.MappingProfiles;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<User, UserModel>();
    }
}