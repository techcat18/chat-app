using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Functions.Results;
using ChatApplication.DAL.Views;
using ChatApplication.Shared.Models;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IChatRepository: IGenericRepository<Chat>
{
    Task<IEnumerable<ChatView>> GetAllByFilterAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatFuncResult>> GetAllByUserIdAsync(
        string userId,
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default);
    Task<Chat?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(Chat chat, CancellationToken cancellationToken = default);
    void Update(Chat chat);
    void Delete(Chat chat);
    Task<int> GetTotalCountAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default);
}