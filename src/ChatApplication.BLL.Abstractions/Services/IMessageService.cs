using ChatApplication.Shared.Models.Message;

namespace ChatApplication.BLL.Abstractions.Services;

public interface IMessageService
{
    Task<IEnumerable<MessageModel>> GetMessagesByChatIdAsync(
        int chatId, 
        CancellationToken cancellationToken = default);

    Task<MessageModel> GetMessageByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
    
    Task<MessageModel> CreateMessageAsync(
        CreateMessageModel createMessageModel,
        CancellationToken cancellationToken = default);
}