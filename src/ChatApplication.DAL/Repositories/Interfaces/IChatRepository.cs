using ChatApplication.DAL.Entities;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IGroupChatRepository: IGenericRepository<Guid, Chat>
{
    Task<Chat?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(Chat chat, CancellationToken cancellationToken = default);
    void Update(Chat chat);
    void Delete(Chat chat);
}