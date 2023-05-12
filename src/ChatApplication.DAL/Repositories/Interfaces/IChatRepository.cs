using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Functions;
using ChatApplication.DAL.Entities.Views;
using ChatApplication.Shared.Models;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IChatRepository: IGenericRepository<Chat>
{
    Task<IEnumerable<ChatView>> GetAllByFilterAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatFunction>> GetAllByUserIdAsync(
        string userId,
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default);
    Task<Chat?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Chat?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task CreateAsync(Chat chat, CancellationToken cancellationToken = default);
    void Update(Chat chat);
    void Delete(Chat chat);
    Task<int> GetTotalCountAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default);
}