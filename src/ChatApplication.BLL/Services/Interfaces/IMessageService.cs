using ChatApplication.BLL.Models.Message;

namespace ChatApplication.BLL.Services.Interfaces;

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