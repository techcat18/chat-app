using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Functions.Results;

namespace ChatApplication.DAL.Repositories.Interfaces;

public interface IMessageRepository: IGenericRepository<Message>
{
    Task<IEnumerable<MessageFuncResult>> GetByChatIdAsync(int chatId, CancellationToken cancellationToken = default);
    Task<MessageFuncResult?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(Message message, CancellationToken cancellationToken = default);
}