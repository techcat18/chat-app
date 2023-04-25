using ChatApplication.Blazor.Models.Message;

namespace ChatApplication.Blazor.Services.Interfaces;

public interface IMessageService
{
    Task<IList<MessageModel>> GetMessagesByChatIdAsync(int chatId);
    Task<MessageModel> CreateMessageAsync(CreateMessageModel createMessageModel);
}