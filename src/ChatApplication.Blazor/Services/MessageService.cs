using ChatApplication.Blazor.Helpers.Interfaces;
using ChatApplication.Blazor.Models.Message;
using ChatApplication.Blazor.Services.Interfaces;
using Flurl.Http;

namespace ChatApplication.Blazor.Services;

public class MessageService: IMessageService
{
    private readonly IApiHelper _apiHelper;

    public MessageService(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<IList<MessageModel>> GetMessagesByChatIdAsync(int chatId)
    {
        var messages = 
            await _apiHelper.GetAsync<IList<MessageModel>>($"chats/{chatId}/messages");
        return messages;
    }

    public async Task<MessageModel> CreateMessageAsync(CreateMessageModel createMessageModel)
    {
        var message = await _apiHelper
            .PostAsync<CreateMessageModel, MessageModel>(createMessageModel, "messages");
        return message;
    }
}