using ChatApplication.Blazor.Models.User;
using ChatApplication.Shared.Models;

namespace ChatApplication.Blazor.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserModel>> GetUsersByFilterAsync(UserFilterModel filterModel);
}