using ChatApplication.Blazor.Helpers.Interfaces;
using ChatApplication.Blazor.Models.Chat;
using ChatApplication.Blazor.Services.Interfaces;
using ChatApplication.Shared.Models;

namespace ChatApplication.Blazor.Services;

public class ChatService: IChatService
{
    private readonly IApiHelper _apiHelper;

    public ChatService(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<IEnumerable<ChatModel>> GetChatsAsync()
    {
        var chats = await _apiHelper.GetAsync<IEnumerable<ChatModel>>("chats/all");
        
        return chats;
    }

    public async Task<IEnumerable<ChatModel>> GetChatsByFilterAsync(ChatFilterModel filterModel)
    {
        var chats =
            await _apiHelper.GetAsync<ChatFilterModel, IEnumerable<ChatModel>>(filterModel, "chats");

        return chats;
    }
}