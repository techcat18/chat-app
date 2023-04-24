using ChatApplication.Blazor.Helpers.Interfaces;
using ChatApplication.Blazor.Models.Chat;
using ChatApplication.Blazor.Services.Interfaces;
using ChatApplication.Shared.Models;
using Newtonsoft.Json;

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
        var chatResponse =
            await _apiHelper.GetAsync(filterModel, "chats");

        var chats = JsonConvert.DeserializeObject<PagedList<ChatModel>>(chatResponse);

        return chats.Data;
    }

    public async Task<ChatModel> GetChatByIdAsync(int id)
    {
        var chatResponse =
            await _apiHelper.GetAsync<ChatModel>($"chats/{id}");

        return chatResponse;
    }
}