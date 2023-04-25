using ChatApplication.DAL.Entities;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IMessageRepository: IGenericRepository<Message>
{
    Task<IEnumerable<Message>> GetByChatIdAsync(int chatId, CancellationToken cancellationToken = default);
    Task<Message?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(Message message, CancellationToken cancellationToken = default);
}