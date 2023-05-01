using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Views;
using ChatApplication.Shared.Models;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<UserView>> GetByFilterAsync(
        UserFilterModel filterModel,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> GetByEmailsAsync(
        IEnumerable<string> emails,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> GetByChatIdAsync(
        int chatId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> GetAllExceptByChatIdAsync(
        int chatId,
        CancellationToken cancellationToken = default);

    Task<int> GetTotalCountAsync(
        UserFilterModel filterModel,
        CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default);

    void Update(User user);
}