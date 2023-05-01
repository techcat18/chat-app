using AutoMapper;
using ChatApplication.BLL.Models.User;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Views;

namespace ChatApplication.BLL.MappingProfiles;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<User, UserModel>();
        CreateMap<UserView, UserModel>();
    }
}