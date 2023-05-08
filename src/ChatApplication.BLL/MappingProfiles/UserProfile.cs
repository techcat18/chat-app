using AutoMapper;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Views;
using ChatApplication.Shared.Models.User;

namespace ChatApplication.BLL.MappingProfiles;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<User, UserModel>();
        CreateMap<UserView, UserModel>();
    }
}