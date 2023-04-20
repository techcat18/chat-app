using ChatApplication.DAL.Entities;
using ChatApplication.Shared.Models;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IChatRepository: IGenericRepository<Chat>
{
    Task<IEnumerable<Chat>> GetAllByFilterAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken);
    Task<Chat?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(Chat chat, CancellationToken cancellationToken = default);
    void Update(Chat chat);
    void Delete(Chat chat);
    Task<int> GetTotalCountAsync(
        ChatFilterModel filterModel,
        CancellationToken cancellationToken = default);
}