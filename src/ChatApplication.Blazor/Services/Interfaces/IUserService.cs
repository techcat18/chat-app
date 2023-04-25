using ChatApplication.Blazor.Models.User;
using ChatApplication.Shared.Models;

namespace ChatApplication.Blazor.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserModel>> GetAllAsync();
    Task<PagedList<UserModel>> GetByFilterAsync(UserFilterModel filterModel);
    Task<UserModel> GetByIdAsync(string id);
}