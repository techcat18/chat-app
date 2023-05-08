using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Entities.Functions;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IMessageRepository: IGenericRepository<Message>
{
    Task<IEnumerable<MessageFunction>> GetByChatIdAsync(int chatId, CancellationToken cancellationToken = default);
    Task<MessageFunction?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(Message message, CancellationToken cancellationToken = default);
}