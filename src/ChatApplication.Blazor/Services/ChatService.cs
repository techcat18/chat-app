using ChatApplication.Blazor.Helpers.Interfaces;
using ChatApplication.Blazor.Models.Chat;
using ChatApplication.Blazor.Services.Interfaces;

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
        var chats = await _apiHelper.GetAsync<IEnumerable<ChatModel>>("chats");
        return chats;
    }
}