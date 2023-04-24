using ChatApplication.BLL.Models.User;
using ChatApplication.Shared.Models;

namespace ChatApplication.BLL.Services.Interfaces;

public interface IUserService
{
    Task<PagedList<UserModel>> GetAllByFilterAsync(
        UserFilterModel filterModel,
        CancellationToken cancellationToken = default);

    Task<UserModel> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default);
}