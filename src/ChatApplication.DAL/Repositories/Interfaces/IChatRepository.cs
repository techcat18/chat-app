using ChatApplication.DAL.Entities;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IChatRepository: IGenericRepository<Chat>
{
    Task<Chat?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(Chat chat, CancellationToken cancellationToken = default);
    void Update(Chat chat);
    void Delete(Chat chat);
}