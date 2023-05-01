using ChatApplication.Blazor.Models.User;
using ChatApplication.Shared.Models;

namespace ChatApplication.Blazor.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserModel>> GetAllAsync();
    Task<PagedList<UserModel>> GetByFilterAsync(UserFilterModel filterModel);
    Task<IEnumerable<UserModel>> GetByChatIdAsync(int chatId);
    Task<IEnumerable<UserModel>> GetAllExceptByChatIdAsync(int chatId);
    Task<UserModel> GetByIdAsync(string id);
    Task UpdateAsync(UpdateUserModel updateUserModel);
}