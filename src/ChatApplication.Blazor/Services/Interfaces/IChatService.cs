using ChatApplication.Blazor.Models.Chat;
using ChatApplication.Shared.Models;

namespace ChatApplication.Blazor.Services.Interfaces;

public interface IChatService
{
    Task<IEnumerable<ChatModel>> GetChatsAsync();
    Task<IEnumerable<ChatModel>> GetChatsByFilterAsync(ChatFilterModel filterModel);
}