using ChatApplication.Shared.Models.Message;

namespace ChatApplication.BLL.Abstractions.Services;

public interface IMessageService
{
    Task<IEnumerable<MessageModel>> GetByChatIdAsync(
        int chatId, 
        CancellationToken cancellationToken = default);

    Task<MessageModel> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
    
    Task<MessageModel> CreateAsync(
        CreateMessageModel createMessageModel,
        CancellationToken cancellationToken = default);
}